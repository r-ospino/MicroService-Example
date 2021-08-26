Microservice Example
==================
Ejemplo de implementación de un microservicio REST en .Net 5.

## Ejecución ##

### Desde Visual Studio ###
1. Abrir la solución con Visual Studio 2019.
2. Dar click derecho sobre la solución y seleccionar "Restore Nuget Packages". 
3. Seleccionar el proyecto "Weelo.RafaelOspino.Api" y presionar <kbd>F5</kbd>.


### Desde Consola ###
**Prerequisito:** Debe tener instalado [.Net 5 SDK](https://dotnet.microsoft.com/download).

1.  Abrir la consola.
2. Ubicarse en el siguiente directorio .\Weelo.RafaelOspino\
3. Ejecutar
   ```
    dotnet restore
   ```
4. Ubicarse en el siguiente directorio .\Weelo.RafaelOspino\src\Weelo.RafaelOspino.Api
5. Ejecutar
   ```
   dotnet run
   ```

## Architecture ##

Estilos y patrones arquitecturales utilizados:

- [Microservices](https://martinfowler.com/articles/microservices.html)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html). Distribución de proyectos en:

  - Domain: Entidades y reglas de negocio.
  - Commons: Utilidades y elementos transversales reutilizables en diferentes proyectos.
  - Infrastructure: Conexión a sistemas externos
  - API: Capa de aplicación y REST API.

- [CQRS](https://martinfowler.com/bliki/CQRS.html) Separación de las estructuras tanto de lectura como de escritura. Implementado usando [Mediatr](https://github.com/jbogard/MediatR).

### Librerías complementarias ###
 - [Flurl](https://flurl.dev/): Invocciones a HttpClient más sencillas y fáciles de probar.
 - [FluentValidations](https://fluentvalidation.net/): Validaciones fluidas y sencillas.
 - [Serilog](https://serilog.net/): Buena librería para escribir logs. Soporta logs estructurados.

##	Structure ##
Para ver el detalle de estructuración del proyecto siga este enlace: [Structure](./docs/Structure.md).


##	Code Documentation ##
El Api REST esta documentado bajo el estándar [OpenAPI Specification](https://swagger.io/specification/) puede consultarla al ejecutar la aplicación para la URL (/api-docs).

El codigo tiene documentación estructurada con etiquetas xml. Los proyectos que componene la solución tiene habilitada la generación del archivo XML con la documentación del codigo. Adicionalmente al interior de los metodos se encuntra documentación para explicar la intención del fragmento de codigo.

##	Best Practices ##

- Comunicación asincrónica.
- Versionamiento del API.
- Logging.
- Healt Check Endpoint (/health).
- Documentación Swagger (/api-docs).
- Separación de responsabilidades.
- Para facilitar la evolución de las operaciones, no se comparten las clases Queries/Commands, Handlers y Responses entre operaciones.
- Propagación del Cancellation Token para poder cancelar en cualquier momento la ejecución de una operación (Ninguna operación es transaccional por lo que la cancelación no traeria efectos secundarios).

##	Manage Performance ##
- Invocaciones asincrónicas para evitar bloqueos de los hilos y poder manejar multiples solicitudes concurrentes.
- No se retornan todas las CriptoMonedas en una sola lista. En lugar de ellos se utiliza paginación y se retorna solo un subcojunto de la información de la CriptoMoneda.
- Uso de una fabrica de HttpClient para reutilizar instancias.
- Se evita el uso de excepciones para controlar el flujo de la aplicación. En su lugar dentro de cada manejador (handler) se realizan validaciones y si no son satisfactorias se retorna un resultado no satisfactorio pero si lanzar excepciones. Las excepciones se utilizan en caso de que el servicio CoinLore no responda satisfactoriamente o que se de un comportamiento inesperado dentro de la aplicación.
- En las invocaciones al servicio CoinLore del se leen los resultados del HttpClient de forma asincrónica.


##	Tests ##

Para ver el detalle de las pruebas unitarias siga este enlace: [Unit Tests](./docs/UnitTests.md).

Para ver el detalle de las pruebas de integración 
siga este enlace: [Integration Tests](./docs/IntegrationTests.md).


##	Security ##

Los consumidores del servicio se autentican al microservicio través de un token [OpenId Connect](https://openid.net/connect/). En una clara separación de responsabilidades el microservicio no emite tokens de autenticación. Dicha responsabilidad recae sobre un [API Gateway](https://microservices.io/patterns/apigateway.html) o un Servicio de token de seguridad ([STS](https://en.wikipedia.org/wiki/Security_token_service)).

Tiene habilitado HTTPS.
