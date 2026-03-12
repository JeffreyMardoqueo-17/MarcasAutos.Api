# Guia de pruebas

Documentos relacionados:

- Arquitectura en capas: [Architecture.md](Architecture.md)
- Configuracion PostgreSQL + EF Core + Docker: [Setup-PostgreSQL-EFCore.md](Setup-PostgreSQL-EFCore.md)
- Vista general del proyecto: [../README.md](../README.md)

Para esta prueba  manejo las pruebas por capas para validar el comportamiento completo, y ademas dejo escenarios de falla controlada para detectar rapidamente si algo se rompe

## Controller

En la capa de controller valido principalmente dos cosas:

- Que al obtener datos todo responda bien cuando el flujo es correcto
- Que tambien exista cobertura de fallo para detectar errores si algo inesperado ocurre 

En esta parte pruebo los escenarios principales de GET y tambien casos de POST para confirmar respuestas correctas en exito y en conflicto-

## Repository

En repository valido que la persistencia funcione correctamente sobre el contexto en memoria

- Verifico lectura y orden esperado de datos
- Verifico existencia por nombre con normalizacion
- Verifico que al crear una marca realmente se guarde en el contexto

La idea aqui es asegurar que la capa de acceso a datos haga exactamente lo que se espera

## Service

En service valido la logica de negocio

- Que al consultar datos devuelva el resultado esperado.
- Que al crear una marca normalice valores antes de guardar.
- Que dispare la regla de negocio cuando se intenta duplicar una marca.

Con esto confirmo que la capa de negocio protege la integridad del dominio

## TestInfrastructure

En infraestructura de pruebas creo el contexto en memoria para aislar cada prueba.

- Cada test arranca con un estado limpio 
- No depende de base de datos externa
- Permite ejecutar pruebas rapidas y repetibles

## Validator (plus)

Como plus, dejo pruebas del validador del modelo de creacion de marca.

- Valido request correcta
- Valido campos obligatorio
- Valido limites minimos y maximos
- Valido formatos invalidos, por ejemplo espacios sin contenido real

Esto complementa el metodo nuevo de crear modelo de auto para que entren datos consistentes desde el inicio.

## Ejecucion de pruebas

Todo se ejecuta desde la raiz del repositorio.

Flujo recomendado:

.\test.ps1

Este comando corre pruebas funcionales y excluye las pruebas de falla intencional.

Si necesito ejecutar algo puntual, uso filtro:

.\test.ps1 -filter "Category=ShouldFail"

.\test.ps1 -filter "MarcaAutoServiceTests"

.\test.ps1 -filter "NombreDelMetodoDePrueba"

## Nota sobre resultados

El archivo .\test.ps1 es basicamente un script para ejecutar las pruebas y ver visualmente los resultados en consola (que pasa y que falla) de forma clara y rapida.

La ventaja es que evita escribir comandos largos cada vez y deja una salida facil de revisar durante el desarrollo.
