using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inaApp.Entities
{
    [Table("tbCategoria")]
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public bool Estado { get; set; } = true; 

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}   