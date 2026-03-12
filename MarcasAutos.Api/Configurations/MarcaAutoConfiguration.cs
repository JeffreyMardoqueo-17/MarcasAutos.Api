using MarcasAutos.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcasAutos.Api.Configurations;

public class MarcaAutoConfiguration : IEntityTypeConfiguration<MarcaAuto>
{
    public void Configure(EntityTypeBuilder<MarcaAuto> entity)
    {
        entity.ToTable("MarcasAutos");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        entity.Property(e => e.PaisOrigen).HasColumnName("pais_origen").HasMaxLength(100);

        entity.HasData(
            new MarcaAuto { Id = 1, Nombre = "Toyota", PaisOrigen = "Japon" },
            new MarcaAuto { Id = 2, Nombre = "Ford", PaisOrigen = "Estados Unidos" },
            new MarcaAuto { Id = 3, Nombre = "BMW", PaisOrigen = "Alemania" }
        );
    }
}
