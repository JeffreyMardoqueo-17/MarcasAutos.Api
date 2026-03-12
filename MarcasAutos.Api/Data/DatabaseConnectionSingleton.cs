using System.Diagnostics.CodeAnalysis;

namespace MarcasAutos.Api.Data;

[ExcludeFromCodeCoverage]
public sealed class DatabaseConnectionSingleton
{
    private static readonly object Sync = new();
    private static DatabaseConnectionSingleton? _instance;

    public string ConnectionString { get; }

    private DatabaseConnectionSingleton(IConfiguration configuration)
    {
        ConnectionString = BuildConnectionString(configuration);
    }

    public static DatabaseConnectionSingleton GetInstance(IConfiguration configuration)
    {
        if (_instance is not null)
            return _instance;

        lock (Sync)
        {
            _instance ??= new DatabaseConnectionSingleton(configuration);
        }

        return _instance;
    }

    private static string BuildConnectionString(IConfiguration configuration)
    {
        var configuredConnection = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(configuredConnection))
            return configuredConnection;
        

        var host = configuration["DB_HOST"] ?? "localhost";
        var port = configuration["DB_PORT"] ?? "5432";
        var database = configuration["DB_NAME"] ?? "marcasautos_db";
        var username = configuration["DB_USER"] ?? "marcasautos_user";

        var password = configuration["DB_PASSWORD"];
        var passwordFile = configuration["DB_PASSWORD_FILE"];

        if (string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(passwordFile) && File.Exists(passwordFile))
            password = File.ReadAllText(passwordFile).Trim();

        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("No se encontro la clave de base de datos. Configure DB_PASSWORD o DB_PASSWORD_FILE.");

        return $"Host={host};Port={port};Database={database};Username={username};Password={password}";
    }
}
