using MarcasAutos.Api.Models.Requests;
using MarcasAutos.Api.Validators;

namespace MarcasAutos.Tests.Validators;

public class CreateMarcaAutoRequestValidatorTests
{
    //estos son validadores pero son mas que todo para el nuevo metodo de post que hice
    private readonly CreateMarcaAutoRequestValidator _validator = new();

    [Fact]
    public async Task ValidateAsync_CuandoLaRequestEsValida_NoRetornaErrores()
    {
        var request = new CreateMarcaAutoRequest
        {
            Nombre = "Honda",
            PaisOrigen = "Japon"
        };

        var result = await _validator.ValidateAsync(request);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task ValidateAsync_CuandoElNombreEstaVacio_RetornaLosErroresEsperados()
    {
        var request = new CreateMarcaAutoRequest
        {
            Nombre = string.Empty,
            PaisOrigen = "Japon"
        };

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Nombre" && error.ErrorMessage == "El nombre de la marca es obligatorio.");
        Assert.Contains(result.Errors, error => error.PropertyName == "Nombre" && error.ErrorMessage == "El nombre de la marca debe tener al menos 2 caracteres.");
    }

    [Fact]
    public async Task ValidateAsync_CuandoElNombreSuperaElMaximoPermitido_RetornaError()
    {
        var request = new CreateMarcaAutoRequest
        {
            Nombre = new string('A', 101),
            PaisOrigen = "Japon"
        };

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Nombre" && error.ErrorMessage == "El nombre de la marca no puede exceder 100 caracteres.");
    }

    [Fact]
    public async Task ValidateAsync_CuandoElPaisOrigenEsSoloEspacios_RetornaError()
    {
        var request = new CreateMarcaAutoRequest
        {
            Nombre = "Mazda",
            PaisOrigen = "   "
        };

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "PaisOrigen" && error.ErrorMessage == "El pais de origen debe tener al menos 2 caracteres validos cuando se envia.");
    }

    [Fact]
    public async Task ValidateAsync_CuandoElPaisOrigenTieneMenosDeDosCaracteres_RetornaError()
    {
        var request = new CreateMarcaAutoRequest
        {
            Nombre = "Mazda",
            PaisOrigen = "A"
        };

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "PaisOrigen" && error.ErrorMessage == "El pais de origen debe tener al menos 2 caracteres validos cuando se envia.");
    }

    [Fact]
    public async Task ValidateAsync_CuandoElPaisOrigenSuperaElMaximoPermitido_RetornaError()
    {
        var request = new CreateMarcaAutoRequest
        {
            Nombre = "Mazda",
            PaisOrigen = new string('B', 101)
        };

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "PaisOrigen" && error.ErrorMessage == "El pais de origen no puede exceder 100 caracteres.");
    }
}
