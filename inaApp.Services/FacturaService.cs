using inaApp.Data;
using inaApp.DTOs;
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

        public async Task<FacturaResponseDTO> CrearFacturaAsync(FacturaCreateDTO dto)
        {
            // 1. Validar Cliente
            var cliente = await _context.Cliente.FindAsync(dto.ClienteId);
            if (cliente == null || !cliente.Activo)
                throw new Exception("El cliente no existe o no está activo.");

            // 2. Validar Número de Factura Único
            if (await _context.Facturas.AnyAsync(f => f.NumeroFactura == dto.NumeroFactura))
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
                    Detalles = detallesEntidades
                };

                _context.Facturas.Add(nuevaFactura);
                await _context.SaveChangesAsync();

                // Actualizar Stock
                foreach (var detalle in detallesEntidades)
                {
                    var prod = await _context.Producto.FindAsync(detalle.ProductoId);
                    prod.Stock -= detalle.Cantidad;
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Retornar respuesta mapeada (simplificado)
                return new FacturaResponseDTO
                {
                    Id = nuevaFactura.Id,
                    NumeroFactura = nuevaFactura.NumeroFactura,
                    Total = nuevaFactura.Total,
                    // ... mapear resto de propiedades
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}