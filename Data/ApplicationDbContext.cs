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


        public DbSet<Producto> Producto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Relación Producto - Categoría
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Relación Cliente - Factura (MAESTRO)
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.Factura)
                .HasForeignKey(f => f.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Relación Factura - FacturaDetalle (MAESTRO - DETALLE)
            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(fd => fd.Factura)
                .WithMany(f => f.FacturaDetalles)
                .HasForeignKey(fd => fd.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);

            // 4. Relación Producto - FacturaDetalle
            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(fd => fd.Producto)
                .WithMany(p => p.FacturaDetalles)
                .HasForeignKey(fd => fd.ProductoId);

            // 5. Configuración de Índices Únicos
            modelBuilder.Entity<Factura>()
                .HasIndex(f => f.NumeroFactura)
                .IsUnique();

            // 6. CONFIGURACIÓN DE DECIMALES (Para evitar advertencias de precisión)
            // Configuramos decimal(18, 4) para Factura
            modelBuilder.Entity<Factura>()
                .Property(f => f.Descuento)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Factura>()
                .Property(f => f.Impuesto)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Factura>()
                .Property(f => f.Subtotal)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Factura>()
                .Property(f => f.Total)
                .HasPrecision(18, 4);

            // Configuramos decimal(18, 4) para FacturaDetalle
            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.Impuesto)
                .HasPrecision(18, 4);

            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.PrecioUnitario)
                .HasPrecision(18, 4);

            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.Subtotal)
                .HasPrecision(18, 4);

            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.TotalLinea)
                .HasPrecision(18, 4);
        }
    }
} 