using MarcasAutos.Api.Entities;
using MarcasAutos.Api.Exceptions;
using MarcasAutos.Api.Repositories;
using MarcasAutos.Api.Services;
using MarcasAutos.Tests.TestInfrastructure;

namespace MarcasAutos.Tests.Services;

public class MarcaAutoServiceTests
{
    [Fact]
    //este verifica que el servicio al consultar las marcas devuelva las marcas
    public async Task GetAllAsync_CuandoSeConsulta_RetornaLasMarcasExistentesOrdenadasPorNombre()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var service = new MarcaAutoService(repository);
        var marcas = (await service.GetAllAsync(CancellationToken.None)).ToList(); //

        Assert.Equal(3, marcas.Count);//verifico que haya 3 marcas
        Assert.Equal(new[] { "BMW", "Ford", "Toyota" }, marcas.Select(marca => marca.Nombre).ToArray());//verifico que las marcas esten ordenadas por nombre
    }

    [Fact]
    public async Task CreateAsync_CuandoLaMarcaEsValida_NormalizaLosCamposYLaPersiste()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var service = new MarcaAutoService(repository);
        var nuevaMarca = new MarcaAuto
        {
            Nombre = "  Nissan  ",
            PaisOrigen = "  Japon  "
        };

        var marcaCreada = await service.CreateAsync(nuevaMarca, CancellationToken.None);

        Assert.True(marcaCreada.Id > 0);
        Assert.Equal("Nissan", marcaCreada.Nombre);
        Assert.Equal("Japon", marcaCreada.PaisOrigen);
    }

    [Fact]
    public async Task CreateAsync_CuandoYaExisteUnaMarcaConElMismoNombre_LanzaBusinessRuleException()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var service = new MarcaAutoService(repository);
        var nuevaMarca = new MarcaAuto
        {
            Nombre = " ford ",
            PaisOrigen = "Estados Unidos"
        };

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() => service.CreateAsync(nuevaMarca, CancellationToken.None));

        Assert.Equal("Ya existe una marca con el mismo nombre.", exception.Message);
    }
}
