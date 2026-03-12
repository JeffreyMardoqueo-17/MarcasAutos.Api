using MarcasAutos.Api.Controllers;
using MarcasAutos.Api.Entities;
using MarcasAutos.Api.Models.Requests;
using MarcasAutos.Api.Models.Responses;
using MarcasAutos.Api.Repositories;
using MarcasAutos.Api.Services;
using MarcasAutos.Tests.TestInfrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MarcasAutos.Tests.Controllers;

/// <summary>
/// Pruebas unitarias del controlador de marcas de autos
/// Estas pruebas validan flujos de GET y POST usando DbContext en memoria Las crusiales son las gets, pero puse como extra el post 
/// </summary>
public class MarcasAutosControllerTests
{
    /// <summary>
    /// Verifica que GET retorne 200 OK osea todo bien
    /// </summary>
    [Fact]
    public async Task GetAll_CuandoExistenMarcas_RetornaOkConLosDatosEsperados()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var service = new MarcaAutoService(repository);
        var controller = new MarcasAutosController(service);

        var actionResult = await controller.GetAll(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var marcas = Assert.IsAssignableFrom<IReadOnlyCollection<MarcaAutoResponse>>(okResult.Value);

        Assert.Equal(3, marcas.Count); //espero 3 marcas
        Assert.Collection( //en esta coleccion espero el las marcas en el orden que espero salgan
            marcas,
            marca =>
            {
                Assert.Equal(3, marca.Id);
                Assert.Equal("BMW", marca.Nombre);
                Assert.Equal("Alemania", marca.PaisOrigen);
            },
            marca =>
            {
                Assert.Equal(2, marca.Id);
                Assert.Equal("Ford", marca.Nombre);
                Assert.Equal("Estados Unidos", marca.PaisOrigen);
            },
            marca =>
            {
                Assert.Equal(1, marca.Id);
                Assert.Equal("Toyota", marca.Nombre);
                Assert.Equal("Japon", marca.PaisOrigen);
            });
    }

    /// <summary>
    /// Verifica que POST con payload valido retorne 201 y persista la nueva marca
    /// </summary>
    [Fact]
    public async Task Create_CuandoLaRequestEsValida_RetornaCreatedAtActionConLaMarcaCreada()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var service = new MarcaAutoService(repository);
        var controller = new MarcasAutosController(service);
        var request = new CreateMarcaAutoRequest
        {
            Nombre = "  Honda  ",
            PaisOrigen = "  Japon  "
        };

        var actionResult = await controller.Create(request, CancellationToken.None);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal(nameof(MarcasAutosController.GetAll), createdAtActionResult.ActionName);

        var response = Assert.IsType<MarcaAutoResponse>(createdAtActionResult.Value); //
        Assert.True(response.Id > 0);
        Assert.Equal("Honda", response.Nombre);
        Assert.Equal("Japon", response.PaisOrigen);

        var marcaPersistida = await context.MarcasAutos.FindAsync(response.Id); //verifico que realmente se guardo en la BD en memoria
        Assert.NotNull(marcaPersistida);
        Assert.Equal("Honda", marcaPersistida.Nombre);
        Assert.Equal("Japon", marcaPersistida.PaisOrigen);
    }

    /// <summary>
    /// Verifica que POST retorne 409 cuando ya existe una marca con el mismo nombre
    /// </summary>
    [Fact]
    public async Task Create_CuandoYaExisteUnaMarcaConElMismoNombre_RetornaConflict()
    {
        await using var context = InMemoryAppDbContextFactory.Create();
        var repository = new MarcaAutoRepository(context);
        var service = new MarcaAutoService(repository);
        var controller = new MarcasAutosController(service);
        var request = new CreateMarcaAutoRequest
        {
            Nombre = " toyota ",
            PaisOrigen = "Japon"
        };

        var actionResult = await controller.Create(request, CancellationToken.None);

        var conflictResult = Assert.IsType<ConflictObjectResult>(actionResult.Result);
        Assert.NotNull(conflictResult.Value);
        Assert.Contains("Ya existe una marca con el mismo nombre.", conflictResult.Value.ToString());
    }

}
