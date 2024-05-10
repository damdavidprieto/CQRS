# Prueba Técnica para candidatos

## Descripción

Este repositorio contiene una prueba técnica para candidatos que deseen unirse a nuestro equipo de desarrollo backend y frontend. El objetivo de la prueba es evaluar las habilidades de los candidatos en el desarrollo de aplicaciones utilizando tecnologías como .NET, C#, SQL Server, MVC...

## Instrucciones

1. Realizar un programa en .NET - C# que cumpla con los siguientes requisitos:
    - Haz un fork de este proyecto
    - Consumir la siguiente API: [https://api.opendata.esett.com/](https://api.opendata.esett.com/). Escoge sólo 1 servicio cualquiera de los proporcionados por la API.
    - Almacenar la información obtenida en la base de datos. (usa SQL Server en contenedor de docker para esto)
    - Implementar un controlador que permita filtrar por Primary Key en la base de datos.
    - Construir una API REST con Swagger que permita visualizar los datos almacenados en la base de datos.
    - Usar contenedores Docker para DDBB y la propia App
    - Usa arquitectura MVC (sólo API imagina que existe un segundo proyecto con el frontend, por tanto las vistas serán DTOs)
    - Haz un pull request con tu nombre completo y comenta lo que creas necesario al evaluador técnico.
    - Elige entre implementar CRUD o CQRS

### Criterios de evaluación:

Se valorará positivamente (pero no es obligatorio cumplir con todos estos puntos):

1. El uso de código limpio y buenas prácticas de programación tanto en el frontend como en el backend.
2. Utilizar código generado a mano en lugar de depender excesivamente de herramientas de generación automática.
3. Hacer commits frecuentes y bien explicados durante el desarrollo.
4. Demostrar conocimientos en patrones de diseño, tanto en el frontend como en el backend.
5. Gestion correcta de los secretos como cadenas de conexión, usuarios, passwords...
6. Uso del inglés en código y comentarios
7. Uso de elementos de monitoreo y observabilidad como ILogger
8. Uso de Eventos
9. Manejo de excepciones con patron monad
10. Pruebas de test

## Tecnologías utilizadas

- .NET - C#
- SQL Server
- MVC

## Estructura del repositorio

No hay restricciones específicas sobre la estructura del repositorio. Los candidatos son libres de organizar su código de la manera que consideren más apropiada. Sin embargo, se recomienda seguir las convenciones de nomenclatura y estructura de proyecto estándar.

¡Buena suerte!
