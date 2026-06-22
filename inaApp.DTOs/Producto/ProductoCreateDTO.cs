using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.DTOs.Producto
{
    public class ProductoCreateDTO
    {

        [Required(ErrorMessage = "El campo es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 10 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El precio no puede ser negativo o 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor a 0")]
        public int Stock { get; set; }

        [StringLength(500, ErrorMessage = "La descripcion no debe pasar de 500 caracteres")]
        public string? Descripcion { get; set; }
        public int CategoriaId { get; set; }
    }
}
