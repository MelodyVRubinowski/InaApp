using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static inaApp.Common.Enums.Enumeradores;

namespace inaApp.DTOs.Cliente
{
    public class ClienteResponseDTO
    {
        public int Id { get; set; }
        public TipoIdentificacion TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
