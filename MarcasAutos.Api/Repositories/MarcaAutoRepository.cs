using MarcasAutos.Api.Data;
using MarcasAutos.Api.Entities;
using MarcasAutos.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Api.Repositories;

public class MarcaAutoRepository : IMarcaAutoRepository
{
    private readonly AppDbContext _dbContext;

    public MarcaAutoRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<MarcaAuto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.MarcasAutos
            .AsNoTracking()
            .OrderBy(marca => marca.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNombreAsync(string nombre, CancellationToken cancellationToken = default)
    {
        var normalizedNombre = nombre.Trim().ToUpper();

        return await _dbContext.MarcasAutos
            .AsNoTracking()
            .AnyAsync(marca => marca.Nombre.ToUpper() == normalizedNombre, cancellationToken);
    }

    public async Task<MarcaAuto> AddAsync(MarcaAuto marcaAuto, CancellationToken cancellationToken = default)
    {
        _dbContext.MarcasAutos.Add(marcaAuto);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return marcaAuto;
    }
}
