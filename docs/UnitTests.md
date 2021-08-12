# Unit Tests #

Para las pruebas unitarias se utilizó:

 - [NUnit](https://nunit.org/) como framework para la ejecución pruebas.
 - [Moq](https://github.com/moq/moq4) para suplantación (Mocks, Stubs, etc.).
 - [AutoFixture](https://github.com/AutoFixture/AutoFixture) Para simplificar la configuración (Setup) y organizacón (Arrange) de las pruebas.
 - [FluentAssertions](https://fluentassertions.com/) Para definir de manera fluida y sencilla las confirmaciones (Asserts).
 - [Coverlet](https://github.com/coverlet-coverage/coverlet) Para obtener métricas de cubrimiento de codigo (Code Coverage)

 Para ejecutar las pruebas unitarias desde la carpeta raíz de la solución "Weelo.RafaelOspino" ejecutar el siguiente comando


## Prerequsitos: ##

Es necesario instalar ReportGenerator como herramienta global para poder crear los reportes de cobertura de codigo, esto solo es necesario realizarlo una sola vez. Desde la carpeta raíz de la solución "Weelo.RafaelOspino" ejecutar los siguientes comandos:

 ```
 dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools
dotnet new tool-manifest
dotnet tool install dotnet-reportgenerator-globaltool
 ```

## Ejecutar pruebas unitarias ##
Desde la carpeta raíz de la solución "Weelo.RafaelOspino" ejecutar el siguiente comando:

 ```
 dotnet test --collect:"XPlat Code Coverage"
 ```

 
## Crear Reporte de Cobertura de Codigo ##
Desde la carpeta raíz de la solución "Weelo.RafaelOspino" ejecutar el siguiente comando:
 ```
 reportgenerator -reports:"*\**\coverage.cobertura.xml" -targetdir:"docs\CoverageReport" -reporttypes:Html;MarkdownSummary
 ```

En la carpeta CoverageReport queda el reporte generado en html. Acá puede ver un [resumen](CoverageReport/Summary.md).