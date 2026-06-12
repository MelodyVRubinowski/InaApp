using inaApp.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("tbCliente")]
[Index(nameof(TipoIdentificacion), nameof(NumeroIdentificacion),IsUnique = true)]
[Index(nameof(CorreoElectronico), IsUnique = true)]
public class Cliente
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCliente { get; set; }

    [Required]
    public int IdTipoIdentificacion { get; set; }

    [Required]
    public TipoIdentificacionEnum TipoIdentificacion { get; set; }

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