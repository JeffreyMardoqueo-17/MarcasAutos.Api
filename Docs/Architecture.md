# Arquitectura usada

El proyecto esta organizado en capas para separar responsabilidades y mantener una estructura limpia.


## Estructura general

```text
MarcasAutos.API
├── MarcasAutos.Api
│   ├── Controllers
│   ├── Services
│   ├── Repositories
│   ├── Interfaces
│   ├── Data
│   ├── Entities
│   ├── Models
│   ├── Validators
│   ├── Configurations
│   └── Migrations
├── MarcasAutos.Tests
│   ├── Controllers
│   ├── Services
│   ├── Repositories
│   ├── Validators
│   └── TestInfrastructure
└── Docs
```

## Relacion con los otros documentos

- Configuracion de base de datos y docker: [Setup-PostgreSQL-EFCore.md](Setup-PostgreSQL-EFCore.md)
- Estrategia de pruebas por capa: [test.md](test.md)
- Vista general del proyecto: [../README.md](../README.md)