using MarcasAutos.Api.Entities;
using MarcasAutos.Api.Repositories;
using MarcasAutos.Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Tests.Repositories;

public class MarcaAutoRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_CuandoExistenRegistros_RetornaLasMarcasOrdenadasPorNombre()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);

        var marcas = (await repository.GetAllAsync(CancellationToken.None)).ToList();
        Assert.Equal(3, marcas.Count);
        Assert.Equal(new[] { "BMW", "Ford", "Toyota" }, marcas.Select(marca => marca.Nombre).ToArray());
    }

    [Fact]
    public async Task ExistsByNombreAsync_CuandoExisteElNombreEnOtroFormato_RetornaTrue()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var exists = await repository.ExistsByNombreAsync("  toyota  ", CancellationToken.None);
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsByNombreAsync_CuandoNoExisteElNombre_RetornaFalse()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var exists = await repository.ExistsByNombreAsync("Hyundai", CancellationToken.None);
        Assert.False(exists);
    }

    [Fact]
    public async Task AddAsync_CuandoSeAgregaUnaMarca_LaPersisteEnLaBaseEnMemoria()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var marca = new MarcaAuto
        {
            Nombre = "Chevrolet",
            PaisOrigen = "Estados Unidos"
        };

        var marcaCreada = await repository.AddAsync(marca, CancellationToken.None);

        Assert.True(marcaCreada.Id > 0);
        Assert.Equal(4, await context.MarcasAutos.CountAsync());
        Assert.Contains(context.MarcasAutos, item => item.Nombre == "Chevrolet");
    }
}
