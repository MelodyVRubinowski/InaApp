using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.DTOs;
using inaApp.DTOs.Factura;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inaApp.Services
{
    public class FacturaService
    {
        private readonly ApplicationDbContext _context;

        public FacturaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FacturaResponseDTO> CrearAsync(FacturaCreateDTO dto)
        {
            // 1. Validar Cliente
            // CORRECCIÓN: Usar dto.ClienteId en lugar de dto.Id
            var cliente = await _context.Cliente.FindAsync(dto.ClienteId);
            if (cliente == null || !cliente.Activo)
                throw new Exception("El cliente no existe o no está activo.");

            // 2. Validar Número de Factura Único
            if (await _context.Factura.AnyAsync(f => f.NumeroFactura == dto.NumeroFactura))
                throw new Exception("El número de factura ya existe.");

            // 3. Validar y Preparar Detalles
            if (dto.Detalles == null || !dto.Detalles.Any())
                throw new Exception("Debe agregar al menos un producto.");

            var detallesEntidades = new List<FacturaDetalle>();
            decimal subtotalGeneral = 0;
            decimal impuestoGeneral = 0;
            const decimal tasaImpuesto = 0.13m;

            foreach (var item in dto.Detalles)
            {
                var producto = await _context.Producto.FindAsync(item.ProductoId);
                if (producto == null || !producto.Estado)
                    throw new Exception($"El producto {item.ProductoId} no existe o no está activo.");

                if (item.Cantidad <= 0)
                    throw new Exception("La cantidad debe ser mayor a cero.");

                if (producto.Stock < item.Cantidad)
                    throw new Exception($"Stock insuficiente para el producto: {producto.Nombre}. Disponible: {producto.Stock}");

                // Cálculos de línea
                decimal subtotalLinea = item.Cantidad * producto.Precio;
                decimal impuestoLinea = subtotalLinea * tasaImpuesto;
                decimal totalLinea = subtotalLinea + impuestoLinea;

                detallesEntidades.Add(new FacturaDetalle
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = producto.Precio,
                    Subtotal = subtotalLinea,
                    Impuesto = impuestoLinea,
                    TotalLinea = totalLinea
                });

                subtotalGeneral += subtotalLinea;
                impuestoGeneral += impuestoLinea;
            }

            // Validar Descuento
            if (dto.Descuento < 0) throw new Exception("El descuento no puede ser negativo.");
            if (dto.Descuento > subtotalGeneral) throw new Exception("El descuento no puede superar el subtotal.");

            decimal totalFinal = subtotalGeneral + impuestoGeneral - dto.Descuento;
            if (totalFinal <= 0) throw new Exception("El total debe ser mayor a cero.");

            // 4. Transacción
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var nuevaFactura = new Factura
                {
                    NumeroFactura = dto.NumeroFactura,
                    Fecha = DateTime.Now,
                    ClienteId = dto.ClienteId, 
                    Subtotal = subtotalGeneral,
                    Impuesto = impuestoGeneral,
                    Descuento = dto.Descuento,
                    Total = totalFinal,
                    Estado = "Activa",
                    FacturaDetalles = detallesEntidades
                };

                _context.Factura.Add(nuevaFactura);
                await _context.SaveChangesAsync();

                // Actualizar Stock
                foreach (var detalle in detallesEntidades)
                {
                    var prod = await _context.Producto.FindAsync(detalle.ProductoId);
                    if (prod != null)
                    {
                        prod.Stock -= detalle.Cantidad;
                    }
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new FacturaResponseDTO
                {
                    NumeroFactura = nuevaFactura.NumeroFactura,
                    Fecha = nuevaFactura.Fecha,
                    Id = nuevaFactura.Id,
                    Total = nuevaFactura.Total,
                    Estado = nuevaFactura.Estado
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error interno al crear factura: {ex.Message}");
            }
        }

        public async Task<FacturaResponseDTO> AnularAsync(int id)
        {
            var factura = await _context.Factura.FindAsync(id);
            if (factura == null)
                throw new Exception("Factura no encontrada.");

            if (factura.Estado == "Anulada")
                throw new Exception("La factura ya está anulada.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                factura.Estado = "Anulada";
                
                // Opcional: Devolver stock a los productos
                foreach (var detalle in factura.FacturaDetalles)
                {
                    var prod = await _context.Producto.FindAsync(detalle.ProductoId);
                    if (prod != null)
                    {
                        prod.Stock += detalle.Cantidad;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new FacturaResponseDTO
                {
                    Id = factura.Id,
                    NumeroFactura = factura.NumeroFactura,
                    Fecha = factura.Fecha,
                    Total = factura.Total,
                    Estado = factura.Estado
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al anular factura: {ex.Message}");
            }
        }

        // Implementar el resto según necesites
        public async Task<FacturaResponseDTO> ObtenerPorIdAsync(int id)
        {
            var factura = await _context.Factura
                .Include(f => f.Cliente)
                .Include(f => f.FacturaDetalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null) throw new Exception("Factura no encontrada.");

            return new FacturaResponseDTO
            {
                Id = factura.Id,
                NumeroFactura = factura.NumeroFactura,
                Fecha = factura.Fecha,
                Subtotal = factura.Subtotal,
                Impuesto = factura.Impuesto,
                Descuento = factura.Descuento,
                Total = factura.Total,
                Estado = factura.Estado
            };
        }

        public async Task<List<FacturaListDTO>> ObtenerTodosAsync()
        {
            return await _context.Factura
                .Include(f => f.Cliente)
                .OrderByDescending(f => f.Fecha)
                .Select(f => new FacturaListDTO
                {
                    Id = f.Id,
                    NumeroFactura = f.NumeroFactura,
                    Fecha = f.Fecha,
                    ClienteNombre = f.Cliente.Nombre,
                    Total = f.Total,
                    Estado = f.Estado
                })
                .ToListAsync();
        }
    }
}