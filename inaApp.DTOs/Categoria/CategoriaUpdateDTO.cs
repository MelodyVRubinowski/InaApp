using System.ComponentModel.DataAnnotations;

namespace inaApp.DTOs.Categoria
{
    public class CategoriaUpdateDTO
    {
        [Required(ErrorMessage = "El ID es obligatorio.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no debe exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        public bool Estado { get; set; } 
    }
}