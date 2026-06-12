using Microsoft.EntityFrameworkCore;
using inaApp.Entites;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Data
{
    public class ApplicationDbContext:DbContext
    {
        //Generar contructor del db context
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        //Entidades para la base de datos
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
    }
}
