using DistribuidoraApi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraApi.Data
{
    // Hereda de DbContext (EF Core), que proporciona la funcionalidad para interactuar con la base de datos.
    public class DistribuidoraDbContext : DbContext
    {
        public DistribuidoraDbContext(DbContextOptions<DistribuidoraDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<UnidadMedida> UnidadesMedida { get; set; }

        // Convenciones globales: todos los decimal serán decimal(18,2) salvo que se indique otra cosa
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Producto → Categoria por código
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)                    // ← el OBJETO, no p.CodigoCategoria
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CodigoCategoria)       // ← aquí sí va el código
                .HasPrincipalKey(c => c.CodigoCategoria);

            // Producto → UnidadMedida por código
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.UnidadMedida)                 // ← el OBJETO, no p.CodigoMedida
                .WithMany()
                .HasForeignKey(p => p.CodigoMedida)          // ← aquí sí va el código
                .HasPrincipalKey(u => u.CodigoMedida);

            // Códigos únicos (Categoria y UnidadMedida ya lo son por HasPrincipalKey)
            modelBuilder.Entity<Producto>().HasIndex(p => p.CodigoProducto).IsUnique();
            modelBuilder.Entity<Cliente>().HasIndex(c => c.CodigoCliente).IsUnique();
            modelBuilder.Entity<Proveedor>().HasIndex(p => p.CodigoProveedor).IsUnique();

            // Stock con 3 decimales
            modelBuilder.Entity<Producto>().Property(p => p.StockActual).HasPrecision(18, 3);
            modelBuilder.Entity<Producto>().Property(p => p.StockMinimo).HasPrecision(18, 3);

            // Sin borrado en cascada
            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            // Unidades de medida iniciales (adapta "Nombre" si tu propiedad se llama distinto)
            modelBuilder.Entity<UnidadMedida>().HasData(
                new UnidadMedida { Id = Guid.Parse("8a1f0c52-3b6e-4d2a-9f1e-1a2b3c4d5e01"), CodigoMedida = "UD", Nombre = "Unidad" },
                new UnidadMedida { Id = Guid.Parse("8a1f0c52-3b6e-4d2a-9f1e-1a2b3c4d5e02"), CodigoMedida = "KG", Nombre = "Kilogramo" },
                new UnidadMedida { Id = Guid.Parse("8a1f0c52-3b6e-4d2a-9f1e-1a2b3c4d5e03"), CodigoMedida = "L", Nombre = "Litro" },
                new UnidadMedida { Id = Guid.Parse("8a1f0c52-3b6e-4d2a-9f1e-1a2b3c4d5e04"), CodigoMedida = "CAJA", Nombre = "Caja" }
            );
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = Guid.Parse("5c2e7a10-9d4b-4f61-8b3a-0e1f2a3b4c01"), CodigoCategoria = "GEN", Nombre = "General" }
            );
        }
    }
}