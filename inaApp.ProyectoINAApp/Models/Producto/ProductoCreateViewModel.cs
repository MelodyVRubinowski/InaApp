using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace inaApp.ProyectoINAApp.Models.Producto
{
    public class ProductoCreateViewModel
    {
        [Required(ErrorMessage = "La categoria Id es un campo obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "La categoria Id debe ser un numero positivo.")]
        public int CategoriaId { get; set; }

        public SelectList? Categorias { get; set; } 

        [Display(Name = "Nombre del producto")]
        [Required(ErrorMessage = "El nombre es un campo obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Precio del producto")]
        [Required(ErrorMessage = "El precio es un campo obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; } = 0;

        [Display(Name = "Stock del producto")]
        [Required(ErrorMessage = "El stock es un campo obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser un numero positivo.")]
        public int Stock { get; set; } = 0;

        [Display(Name = "Descripcion del producto")]
        [StringLength(500, ErrorMessage = "La descripcion NO debe superar los 500 caracterres.")]
        public string? Descripcion { get; set; } = string.Empty;

    }
}
