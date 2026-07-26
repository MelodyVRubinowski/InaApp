using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace inaApp.DTOs
{
    public class FacturaDetalleCreateDTO
    {
        [Required(ErrorMessage = "El Producto es obligatorio.")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La Cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El Precio Unitario es obligatorio.")]
        [Range(0.01, 999999999.99, ErrorMessage = "El precio unitario debe ser positivo.")]
        public decimal PrecioUnitario { get; set; }

        // Si el detalle tiene descuento propio (además del de la factura), descomenta esto:
        // [Range(0, 100, ErrorMessage = "El descuento no puede ser negativo.")]
        // public decimal DescuentoLinea { get; set; }
    }
}