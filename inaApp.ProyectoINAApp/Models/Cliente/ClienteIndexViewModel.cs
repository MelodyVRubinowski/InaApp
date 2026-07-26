using static inaApp.Common.Enums.Enumeradores;

namespace InaApp.ProyectoInaApp.Models.Cliente
{
    public class ClienteIndexViewModel
    {
        public int Id { get; set; }

        public string NumeroIdentificacion { get; set; } = string.Empty;

        public TipoIdentificacion TipoIdentificacion { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string PrimerApellido { get; set; } = string.Empty;

        public string? SegundoApellido { get; set; } = string.Empty;

        public string? Correo { get; set; } = string.Empty;

        public string? Telefono { get; set; } = string.Empty;

        public DateOnly FechaNacimiento { get; set; }
    }
}
