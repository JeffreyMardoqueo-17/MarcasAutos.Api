using MarcasAutos.Api.Controllers;
using MarcasAutos.Api.Models.Requests;
using MarcasAutos.Api.Models.Responses;
using MarcasAutos.Api.Repositories;
using MarcasAutos.Api.Services;
using MarcasAutos.Tests.TestInfrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MarcasAutos.Tests.Controllers;

/// <summary>
/// Tests intencionalmente fallidos del controlador de marcas de autos
/// Todos los tests aqui tienen el trait "Category=ShouldFail" y son excluidos del script run-tests.ps1.
/// </summary>
public class MarcasAutosControllerShouldFailTests
{
    /// <summary>
    /// este test esta para validar que falle, 
    /// Se espera 999 marcas pero la BD en memoria solo contiene 3 del seed
    /// </summary>
    [Fact]
    [Trait("Category", "ShouldFail")]
    public async Task ShouldFail_GetAll_ElConteoNoCoincide_ParaDemostracion()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var service = new MarcaAutoService(repository);
        var controller = new MarcasAutosController(service);

        var actionResult = await controller.GetAll(CancellationToken.None);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var marcas = Assert.IsAssignableFrom<IReadOnlyCollection<MarcaAutoResponse>>(okResult.Value);

        // aqui le digo que espero 999 perooooooo realmente son 3 Obviamebte
        Assert.Equal(999, marcas.Count);
    }

    /// <summary>
    /// Se espera el nombre "NombreIncorrecto" pero el servicio guarda "Honda"
    /// </summary>
    [Fact]
    [Trait("Category", "ShouldFail")]
    public async Task ShouldFail_Create_NombreEsperadoIncorrecto_ParaDemostracion()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var service = new MarcaAutoService(repository);
        var controller = new MarcasAutosController(service);

        var request = new CreateMarcaAutoRequest
        {
            Nombre = "Honda",
            PaisOrigen = "Japon"
        };

        var actionResult = await controller.Create(request, CancellationToken.None);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var response = Assert.IsType<MarcaAutoResponse>(createdAtActionResult.Value);

        // Falla a proposito el nombre real es Honda.
        Assert.Equal("NombreIncorrecto", response.Nombre);
    }
}
