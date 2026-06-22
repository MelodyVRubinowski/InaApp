using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static inaApp.Common.Enums.Enumeradores;

namespace inaApp.DTOs.Cliente
{
    public class ClienteUpdateDTO
    {
        [Required(ErrorMessage = "El ID es obligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de identificación es obligatorio")]
        public TipoIdentificacion TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "El número de identificación es obligatorio")]
        [MaxLength(20, ErrorMessage = "El número de identificación no debe exceder los 20 caracteres")]
        public string NumeroIdentificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no debe exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [MaxLength(50, ErrorMessage = "El primer apellido no debe exceder los 50 caracteres")]
        public string PrimerApellido { get; set; }

        [MaxLength(50, ErrorMessage = "El segundo apellido no debe exceder los 50 caracteres")]
        public string? SegundoApellido { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [MaxLength(150, ErrorMessage = "El correo electrónico no debe exceder los 150 caracteres")]
        public string? CorreoElectronico { get; set; }

        [Phone(ErrorMessage = "El teléfono no es válido")]
        [MaxLength(20, ErrorMessage = "El teléfono no debe exceder los 20 caracteres")]
        public string? Telefono { get; set; }
    }
}
