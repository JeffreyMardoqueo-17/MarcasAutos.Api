using MarcasAutos.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<MarcaAuto> MarcasAutos => Set<MarcaAuto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MarcaAuto>(entity =>
        {
            entity.ToTable("marcas_autos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            entity.Property(e => e.PaisOrigen).HasColumnName("pais_origen").HasMaxLength(100);

            entity.HasData(
                new MarcaAuto { Id = 1, Nombre = "Toyota", PaisOrigen = "Japon" },
                new MarcaAuto { Id = 2, Nombre = "Ford", PaisOrigen = "Estados Unidos" },
                new MarcaAuto { Id = 3, Nombre = "BMW", PaisOrigen = "Alemania" }
            );
        });
    }
}
