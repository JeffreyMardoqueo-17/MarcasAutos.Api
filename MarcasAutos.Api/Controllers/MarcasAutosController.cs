using MarcasAutos.Api.Exceptions;
using MarcasAutos.Api.Entities;
using MarcasAutos.Api.Interfaces;
using MarcasAutos.Api.Models.Requests;
using MarcasAutos.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MarcasAutos.Api.Controllers;

[ApiController]
[Route("/[controller]")]
public class MarcasAutosController : ControllerBase
{
    private readonly IMarcaAutoService _marcaAutoService;

    public MarcasAutosController(IMarcaAutoService marcaAutoService)
    {
        _marcaAutoService = marcaAutoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<MarcaAutoResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<MarcaAutoResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var marcas = await _marcaAutoService.GetAllAsync(cancellationToken);
        var response = marcas
            .Select(marca => new MarcaAutoResponse(
                marca.Id,
                marca.Nombre.Trim(),
                string.IsNullOrWhiteSpace(marca.PaisOrigen) ? null : marca.PaisOrigen.Trim()))
            .ToList();

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MarcaAutoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MarcaAutoResponse>> Create(
        [FromBody] CreateMarcaAutoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var marcaAuto = new MarcaAuto
            {
                Nombre = request.Nombre,
                PaisOrigen = request.PaisOrigen
            };

            var createdMarca = await _marcaAutoService.CreateAsync(marcaAuto, cancellationToken);

            var response = new MarcaAutoResponse(
                createdMarca.Id,
                createdMarca.Nombre,
                createdMarca.PaisOrigen);

            return CreatedAtAction(
                nameof(GetAll),
                null,
                response);
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
