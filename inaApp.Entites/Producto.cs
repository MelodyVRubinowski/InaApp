using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inaApp.Entites
{
    [Table("tbProducto")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nombre { get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public string Descripcion { get; set; }

        public bool Estado { get; set; }
    }
}
