using System.ComponentModel.DataAnnotations;

namespace inaApp.ProyectoINAApp.Models.Categoria
{
    public class CategoriaEditViewModel
    {
        [Required(ErrorMessage = "El Id es un campo obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id debe ser un numero positivo.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es un campo obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
