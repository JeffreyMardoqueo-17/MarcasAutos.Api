using MarcasAutos.Api.Entities;

namespace MarcasAutos.Api.Interfaces;

public interface IMarcaAutoService
{
    Task<IEnumerable<MarcaAuto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<MarcaAuto> CreateAsync(MarcaAuto marcaAuto, CancellationToken cancellationToken = default);
}
