using FluentValidation;
using FluentValidation.AspNetCore;
using MarcasAutos.Api.Data;
using MarcasAutos.Api.Interfaces;
using MarcasAutos.Api.Repositories;
using MarcasAutos.Api.Services;
using MarcasAutos.Api.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var dbConnectionSingleton = DatabaseConnectionSingleton.GetInstance(builder.Configuration);
builder.Services.AddSingleton(dbConnectionSingleton);

builder.Services.AddDbContext<AppDbContext>((_, options) =>
    options.UseNpgsql(dbConnectionSingleton.ConnectionString));

builder.Services.AddScoped<IMarcaAutoRepository, MarcaAutoRepository>();
builder.Services.AddScoped<IMarcaAutoService, MarcaAutoService>();

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateMarcaAutoRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MarcasAutos API",
        Version = "v1",
        Description = "API para gestion de marcas de autos con EF Core y PostgreSQL. Peticiones Get y Post "
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");

    const int maxAttempts = 10;
    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            logger.LogInformation("Conexion a PostgreSQL verificada.");
            break;
        }

        if (attempt == maxAttempts)
            throw new InvalidOperationException("No fue posible conectar con PostgreSQL al iniciar la API.");

        logger.LogWarning("Intento {Attempt}/{MaxAttempts}: PostgreSQL aun no responde. Reintentando...", attempt, maxAttempts); //esto es para que cuando se levante todo, espere a que el contenedor de postgres este listo antes de intentar aplicar las migraciones
        await Task.Delay(2000);
    }

    await dbContext.Database.MigrateAsync();
    logger.LogInformation("Migraciones EF Core aplicadas correctamente."); //esto es para que cuando se levante todo, se ejecute una migracoin automaticamente para crear la tabla de lasMarcas de autos y agregar los datos de seed
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MarcasAutos API v1");
    options.DocumentTitle = "MarcasAutos API - Swagger";
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/swagger/index.html"))
    .ExcludeFromDescription();
app.MapControllers();

app.Run();
