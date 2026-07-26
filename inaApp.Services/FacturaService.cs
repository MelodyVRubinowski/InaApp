using inaApp.Services.Interfaces;
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
    public class FacturaService : IFacturaService
    {
        private readonly ApplicationDbContext _context;

        public FacturaService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retorna Task<FacturaResponseDTO> directamente
        public async Task<FacturaResponseDTO> CrearFacturaAsync(FacturaCreateDTO dto)
        {
            // 1. Validar Cliente
            var cliente = await _context.Cliente.FindAsync(dto.Id);
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
                    Id = dto.Id,
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

                // Retornar el DTO directamente
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

        // Implementación de los métodos faltantes (deben existir)
        public async Task<FacturaResponseDTO> AnularFacturaAsync(int id)
        {
            throw new NotImplementedException("Método AnularFacturaAsync no implementado.");
        }

        public async Task<FacturaResponseDTO> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException("Método ObtenerPorIdAsync no implementado.");
        }

        public async Task<List<FacturaListDTO>> ObtenerTodasAsync()
        {
            throw new NotImplementedException("Método ObtenerTodasAsync no implementado.");
        }

        public (decimal Subtotal, decimal Impuesto, decimal Total) CalcularTotales(List<FacturaDetalleCreateDTO> detalles, decimal descuento = 0)
        {
            throw new NotImplementedException("Método CalcularTotales no implementado.");
        }
    }
}