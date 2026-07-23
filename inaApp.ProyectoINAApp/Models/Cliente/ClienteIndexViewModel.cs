using System.ComponentModel.DataAnnotations;

namespace inaApp.ProyectoINAApp.Models.Cliente
{
    public class ClienteIndexViewModel
    {
       
        [Display(Name = "Tipo Identificación")]
        public string TipoIdentificacion { get; set; } = string.Empty;

        [Display(Name = "Número Identificación")]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Display(Name = "Correo")]
        public string? CorreoElectronico { get; set; }

        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

       
    }
}