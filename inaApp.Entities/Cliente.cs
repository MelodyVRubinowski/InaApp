using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static inaApp.Common.Enums.Enumeradores;
using Microsoft.EntityFrameworkCore; 

namespace inaApp.Entities
{
    [Table(name: "tbCliente")]
    [Index(
        nameof(TipoIdentificacion),
        nameof(NumeroIdentificacion),
        IsUnique = true)]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public TipoIdentificacion TipoIdentificacion { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string PrimerApellido { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? SegundoApellido { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? CorreoElectronico { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}