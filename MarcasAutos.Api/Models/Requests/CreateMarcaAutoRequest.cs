namespace MarcasAutos.Api.Models.Requests;

public sealed record class CreateMarcaAutoRequest
{
    public string Nombre { get; init; } = string.Empty;

    public string? PaisOrigen { get; init; }
}
