using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Entites
{
    [Table(name:"tbEmpleado")]
    public class Empleado
    {
        [Key]
        private int Id { get; set; }

        [Column("name")]
        private string Name { get; set; }


        [Column("edad")]
        private int Edad { get; set; }

        [Column("primerApellido")]
        private string apellido1 { get; set; }


        [Column("segundoApellido")]
        private string apellido2 { get; set; }
    }
}
