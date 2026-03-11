## Arquitectura usada 
```
MarcasAutos
│
├── MarcasAutos.Api
│   │
│   ├── Controllers
│   │   └── MarcasAutosController.cs
│   │
│   ├── Data
│   │   ├── AppDbContext.cs
│   │   └── DbInitializer.cs
│   │
│   ├── Entities
│   │   └── MarcaAuto.cs
│   │
│   ├── Interfaces
│   │   ├── IMarcaAutoRepository.cs
│   │   └── IMarcaAutoService.cs
│   │
│   ├── Repositories
│   │   └── MarcaAutoRepository.cs
│   │
│   ├── Services
│   │   └── MarcaAutoService.cs
│   │
│   ├── Configurations
│   │   └── MarcaAutoConfiguration.cs
│   │
│   ├── Migrations
│   │
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Program.cs
│   └── Dockerfile
│
├── MarcasAutos.Tests
│   │
│   ├── Controllers
│   │   └── MarcasAutosControllerTests.cs
│   │
│   ├── Services
│   │   └── MarcaAutoServiceTests.cs
│   │
│   ├── Data
│   │   └── InMemoryDbContextFactory.cs
│   │
│   └── MarcasAutos.Tests.csproj
│
├── Docs
├── compose.yml
├── .gitignore
├── README.md
└── MarcasAutos.sln
```