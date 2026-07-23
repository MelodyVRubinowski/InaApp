using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static inaApp.Common.Enums.Enumeradores;

namespace inaApp.ProyectoINAApp.Models.Cliente
{
    public class ClienteEditViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Tipo de identificación")]
        [Required(ErrorMessage = "El tipo de identificación es obligatorio.")]
        public TipoIdentificacion TipoIdentificacion { get; set; }

        public SelectList? TiposIdentificacion { get; set; }

        [Display(Name = "Número de identificación")]
        [Required(ErrorMessage = "El número de identificación es obligatorio.")]
        [StringLength(20, ErrorMessage = "El número de identificación no debe exceder los 20 caracteres.")]
        
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no debe exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Primer apellido")]
        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El primer apellido no debe exceder los 50 caracteres.")]
        public string PrimerApellido { get; set; } = string.Empty;

        [Display(Name = "Segundo apellido")]
        [StringLength(50, ErrorMessage = "El segundo apellido no debe exceder los 50 caracteres.")]
        public string? SegundoApellido { get; set; }

        [Display(Name = "Correo electrónico")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [StringLength(150, ErrorMessage = "El correo electrónico no debe exceder los 150 caracteres.")]
        public string? CorreoElectronico { get; set; }

        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "El teléfono no es válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no debe exceder los 20 caracteres.")]
        public string? Telefono { get; set; }
    }
}
