# Structure #

## Folder Structure ##
La estructura de carpetas utilizadas para la solución es la siguiente:

    .
    ├── docs                    # Documentación 
    ├── src                     # Código fuente
    ├── tests                   # Pruebas unitarias
    ├── tools                   # Herramientas del proyecto
    └── README.md

## Namespace Naming Convention ##

Los espacios de nombres (namespaces) del proyecto tienen la siguiente convención de nomenclatura:

```
CompanyName.ProjectName.Responsabilidad.Caracteristica
```

Ejemplo
```
Weelo.RafaelOspino.API.Features.CryptoCurrencyFeatures
```

_Los niveles de espacios de nombres, salvo los dos primeros (CompanyName.ProjectName), se ven reflejdos en estructura de carpetas dentro del proyecto._

## Project Naming Convention ##


Los nombres de los proyectos se dan de la siguiente forma:

```
CompanyName.ProjectName.Responsabilidad
```
en el caso de las pruebas unitaria se adiciona el sufijo 'UnitTests'

Ejemplo:
```
Weelo.RafaelOspino.API
Weelo.RafaelOspino.API.UnitTests
```
