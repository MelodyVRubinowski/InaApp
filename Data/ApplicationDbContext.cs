using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet (Plural es mejor práctica, aunque singular funciona)
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Relación Producto - Categoría (Ya la tenías, la ajusto ligeramente)
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict); // Evita borrar categoría si tiene productos

            // 2. Relación Cliente - Factura (MAESTRO)
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.Facturas) // Asegúrate que Cliente tenga: public List<Factura> Facturas { get; set; }
                .HasForeignKey(f => f.ClienteId) // La FK en Factura
                .OnDelete(DeleteBehavior.Restrict); // No borrar cliente si tiene facturas

            // 3. Relación Factura - FacturaDetalle (MAESTRO - DETALLE)
            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(fd => fd.Factura)
                .WithMany(f => f.Detalles) // Asegúrate que Factura tenga: public List<FacturaDetalle> Detalles { get; set; }
                .HasForeignKey(fd => fd.FacturaId) // La FK en FacturaDetalle
                .OnDelete(DeleteBehavior.Cascade); // Si borras factura, se borran sus detalles

            // 4. Relación Producto - FacturaDetalle
            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(fd => fd.Producto)
                .WithMany(p => p.FacturaDetalles) // Asegúrate que Producto tenga: public List<FacturaDetalle> FacturaDetalles { get; set; }
                .HasForeignKey(fd => fd.ProductoId);

            // 5. Configuración de Índices Únicos (Evitar números de factura duplicados)
            modelBuilder.Entity<Factura>()
                .HasIndex(f => f.NumeroFactura)
                .IsUnique();
        }
    }
}