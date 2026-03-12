using MarcasAutos.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Tests.TestInfrastructure;

internal static class InMemoryAppDbContextFactory
{
//esto de aqui es para crear un contexto de base de datos en memoria  
    public static AppDbContext Create(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;
            //Aqui hago que se borre la BD en memoria cada vez que se crea un nuevo contexto, asi cada prueba tiene algo limpio
        var context = new AppDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }
}
