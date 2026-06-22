using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace inaApp.Entities

//nivel acceso
//public: cualquiera accede a la clase
//private: solo las clases dentro del mismo archivo pueden acceder a la clase 
//internal: solo pueden acceder clases dentro del mismo proyecto
//protected: solo clases dentro del mismo proyecto o heredadas
{

    [Table(name: "tbProducto")]
    public class Producto
    {
        //propiedades : variables que describen un objeto
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 10 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, int.MaxValue, ErrorMessage = "El precio no puede ser negativo o 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor a 0")]
        public int Stock { get; set; }

        [StringLength(500,ErrorMessage = "La descripcion no debe pasar de 500 caracteres")]
        public string? Descripcion { get; set; }
        public bool Estado { get; set; } = true;

        //relacion con categoria (muchos a uno)
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

    }
}
