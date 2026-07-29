using System.ComponentModel.DataAnnotations;

namespace inaApp.ProyectoINAApp.Models.Producto
{
    public class ProductoPopUpViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Producto")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Categoría")]
        public string CategoriaNombre { get; set; } = string.Empty;

        [Display(Name = "Precio Unitario")]
        public decimal Precio { get; set; }

        [Display(Name = "Impuesto")]
        public string ImpuestoAplicable { get; set; } = string.Empty;

        [Display(Name = "% Imp.")]
        public decimal PorcentajeImpuesto { get; set; }

        [Display(Name = "Existencias")]
        public int Stock { get; set; }

        public decimal DescuentoMaximoPermitido { get; set; }
    }
}