using MarcasAutos.Api.Entities;
using MarcasAutos.Api.Exceptions;
using MarcasAutos.Api.Interfaces;

namespace MarcasAutos.Api.Services;

public class MarcaAutoService : IMarcaAutoService
{
    private readonly IMarcaAutoRepository _marcaAutoRepository;

    public MarcaAutoService(IMarcaAutoRepository marcaAutoRepository)
    {
        _marcaAutoRepository = marcaAutoRepository;
    }


    public Task<IEnumerable<MarcaAuto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _marcaAutoRepository.GetAllAsync(cancellationToken);
    }

    public async Task<MarcaAuto> CreateAsync(MarcaAuto marcaAuto, CancellationToken cancellationToken = default)
    {
        var nombreNormalizado = marcaAuto.Nombre.Trim();
        var paisOrigenNormalizado = string.IsNullOrWhiteSpace(marcaAuto.PaisOrigen)
            ? null
            : marcaAuto.PaisOrigen.Trim();

        if (await _marcaAutoRepository.ExistsByNombreAsync(nombreNormalizado, cancellationToken))
            throw new BusinessRuleException("Ya existe una marca con el mismo nombre.");

        marcaAuto.Nombre = nombreNormalizado;
        marcaAuto.PaisOrigen = paisOrigenNormalizado;

        return await _marcaAutoRepository.AddAsync(marcaAuto, cancellationToken);
    }
}
