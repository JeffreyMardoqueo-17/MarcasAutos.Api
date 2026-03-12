# Configuracion EF Core + PostgreSQL + Docker

Resumen de la configuracion aplicada para ejecutar API + PostgreSQL con un solo comando y crear esquema/seed por migraciones 

## 1) Paquetes instalados

Todos los paquetes de EF Core/Npgsql quedaron en version 8, alineados con .NET 8.

- `Microsoft.EntityFrameworkCore` (8.0.8)
- `Microsoft.EntityFrameworkCore.Design` (8.0.8)
- `Npgsql.EntityFrameworkCore.PostgreSQL` (8.0.8)
- `Swashbuckle.AspNetCore` (6.6.2)

Comandos usados:

```powershell
dotnet add "MarcasAutos.Api/MarcasAutos.Api.csproj" package Microsoft.EntityFrameworkCore --version 8.0.8
dotnet add "MarcasAutos.Api/MarcasAutos.Api.csproj" package Microsoft.EntityFrameworkCore.Design --version 8.0.8
dotnet add "MarcasAutos.Api/MarcasAutos.Api.csproj" package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.8
```

## 2) Donde se configuro la conexion

- Se define por variables de entorno en `compose.yml`:
  - `DB_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER`, `DB_PASSWORD`
- `appsettings.json` y `appsettings.Development.json` se dejaron sin secretos en `ConnectionStrings:DefaultConnection`.
- La cadena final se construye en `MarcasAutos.Api/Data/DatabaseConnectionSingleton.cs`.

## 3) Patron Singleton para conexion

Se implemento un singleton explicito en

- `MarcasAutos.Api/Data/DatabaseConnectionSingleton.cs`

Responsabilidades:

- Construir una sola vez la cadena de conexion por proces
- Permitir lectura de `DB_PASSWORD` o `DB_PASSWORD_FILE`
- Exponer `ConnectionString` para registrar `AppDbContext`

Nota :

- El singleton aplica a la configuracion de conexion.
- `AppDbContext` sigue con ciclo de vida `Scoped` (recomendado por EF Core)

## 4) Program.cs (resumen)

Lo que congigure en el  `Program.cs` es

- Registro de singleton de conexion.
- Registro de `AppDbContext` con Npgsql.
- Reintentos de conectividad a PostgreSQL al iniciar (`Database.CanConnectAsync()`).
- Aplicacion automatica de migraciones en startup (`Database.MigrateAsync()`).
- Swagger con metadata y UI.

## 5) Migraciones y Seed Data (nuevo)

### 5.1 Modelo con seed

En `MarcasAutos.Api/Data/AppDbContext.cs` se configuro `HasData` para `MarcaAuto` con 3 registros:

- Toyota (Japon)
- Ford (Estados Unidos)
- BMW (Alemania)

### 5.2 Migracion creada

Se genero la migracion:

- `MarcasAutos.Api/Migrations/20260312001801_InitialCreateMarcaAuto.cs`
- `MarcasAutos.Api/Migrations/AppDbContextModelSnapshot.cs`

Esta migracion crea la tabla `marcas_autos` e inserta el seed data.

## 6) Docker y seed-data

Archivos clave

- `compose.yml`
- `Docker/PostgreSQL/Dockerfile`
- `Docker/PostgreSQL/seed-data/001-seed-data.sql`
- `start-compose.ps1`

Nota : 
- El archivo ``compose.yml`` se llama asi porque segun la documentacion asi puede llamarse ahora


## 7) Ejecucion en un comando

Opcion recomendada (con validaciones y mensajes):

```powershell
powershell -ExecutionPolicy Bypass -File .\start-compose.ps1 -ApiPort 8080
```

Opcion directa:

```powershell
docker compose up -d
```

## 8) Evidencias esperadas

En logs deben aparecer mensajes como:

- PostgreSQL: `database system is ready to accept connections`
- API: `Conexion a PostgreSQL verificada.`
- API: `Migraciones EF Core aplicadas correctamente.`
- API escuchando en puerto `8080`


Resultado esperado: 3 filas (Toyota, Ford, BMW).
