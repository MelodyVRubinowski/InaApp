using System.ComponentModel.DataAnnotations;

namespace inaApp.DTOs.Producto
{
    public class ProductoCreateDTO
    {
        [Required(ErrorMessage = "El campo es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio no puede ser negativo o 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [StringLength(500, ErrorMessage = "La descripcion no debe pasar de 500 caracteres")]
        public string? Descripcion { get; set; }

        public int CategoriaId { get; set; }

        // --- NUEVOS CAMPOS REQUERIDOS ---

        [Required(ErrorMessage = "El impuesto aplicable es obligatorio")]
        public string ImpuestoAplicable { get; set; } = "IVA";

        [Required(ErrorMessage = "El porcentaje de impuesto es obligatorio")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        public decimal PorcentajeImpuesto { get; set; }

        [Required(ErrorMessage = "El descuento máximo es obligatorio")]
        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal DescuentoMaximoPermitido { get; set; }
    }
}