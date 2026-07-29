using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inaApp.Entities
{
    [Table(name: "tbProducto")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, int.MaxValue, ErrorMessage = "El precio no puede ser negativo o 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no debe pasar de 500 caracteres")]
        public string? Descripcion { get; set; }

        public bool Estado { get; set; } = true;

        // --- NUEVOS CAMPOS REQUERIDOS POR LA ACTIVIDAD ---

        [Required(ErrorMessage = "El impuesto aplicable es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre del impuesto es muy largo")]
        public string ImpuestoAplicable { get; set; } = "IVA"; // Ejemplo: IVA, Exento, etc. [1]

        [Required(ErrorMessage = "El porcentaje de impuesto es obligatorio")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        public decimal PorcentajeImpuesto { get; set; } // [1]

        [Required(ErrorMessage = "El descuento máximo es obligatorio")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal DescuentoMaximoPermitido { get; set; } // [1]

        // --- RELACIONES ---

        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
        public required List<FacturaDetalle> FacturaDetalles { get; set; }
    }
}