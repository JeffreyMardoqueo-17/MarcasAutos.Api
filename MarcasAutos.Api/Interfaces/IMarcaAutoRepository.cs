using MarcasAutos.Api.Entities;

namespace MarcasAutos.Api.Interfaces;

public interface IMarcaAutoRepository
{
    Task<IEnumerable<MarcaAuto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsByNombreAsync(string nombre, CancellationToken cancellationToken = default);

    Task<MarcaAuto> AddAsync(MarcaAuto marcaAuto, CancellationToken cancellationToken = default);
}
