# MarcasAutos.API

API en .NET 8 con arquitectura en capas, separando responsabilidades para mantener el proyecto claro, testeable y facil de mantener.

## Arquitectura en capas

- Controller: recibe requests HTTP y devuelve respuestas HTTP
- Service: aplica reglas de negocio y orquesta el flujo 
- Repository: acceso a datos y consultas a base de datos es su unico traabajo
- Data: configuracion de EF Core, contexto y conexion
- Validator: valida el modelo de entrada antes del flujo de negocio

Para ver el detalle completo de arquitectura y estructura:

- [Docs/Architecture.md](Docs/Architecture.md)

## Documentacion

- Arquitectura por capas: [Docs/Architecture.md](Docs/Architecture.md)
- Configuracion PostgreSQL + EF Core + Docker: [Docs/Setup-PostgreSQL-EFCore.md](Docs/Setup-PostgreSQL-EFCore.md)
- Estrategia y ejecucion de pruebas: [Docs/test.md](Docs/test.md)
## 📦 Despliegue
- Vista general del despliegue: [Docs/Despliegue.md](Docs/Despliegue.md)

## Requisitos

- .NET SDK 8.0
- Docker Desktop (con Docker Compose)

## Ejecutar el proyecto

Desde la raiz del repositorio, ejecuta:

```powershell
docker compose up -d --build
```

La API quedara disponible en:

- http://localhost:8080
- http://localhost:8080/swagger/index.html

Para detener los contenedores:

```powershell
docker compose down
```

Para limpiar volumenes (opcional, reinicia datos):

```powershell
docker compose down -v
```

## Ejecutar tests

### Comando recomendado (simple)

Desde la raiz del repositorio:

```powershell
.\test.ps1
```

Esto ejecuta los tests funcionales y excluye los tests de demostracion/falla intencional (Category=ShouldFail).

### Ejecutar por filtro (opcional)

Puedes pasar un filtro especifico para correr solo una categoria, clase o metodo:

```powershell
.\test.ps1 -filter "Category=ShouldFail"
.\test.ps1 -filter "MarcaAutoServiceTests"
.\test.ps1 -filter "CreateAsync_CuandoLaMarcaEsValida_NormalizaLosCamposYLaPersiste"
```

### Comando directo con dotnet test (alternativo)

Esto ejecuta sin el script.

```powershell
dotnet test .\MarcasAutos.Tests\MarcasAutos.Tests.csproj --filter "Category!=ShouldFail" --logger "console;verbosity=normal"
```

Para mas detalle sobre pruebas por capa y objetivo de cada suite:

- [Docs/test.md](Docs/test.md)
