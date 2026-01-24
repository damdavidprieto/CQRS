# 🏗️ Backend Architecture: Clean Architecture + CQRS (.NET)

![Status: Archived](https://img.shields.io/badge/Status-Archivado%20%2F%20Research-orange?style=flat-square)
![Tech: .NET 7](https://img.shields.io/badge/Stack-.NET%207-512bd4?style=flat-square)
![Pattern: CQRS](https://img.shields.io/badge/Pattern-CQRS-blue?style=flat-square)
![Architecture: Clean](https://img.shields.io/badge/Arch-Clean%20Architecture-green?style=flat-square)

> **Nota de Archivo**
> Este repositorio constituye un ejemplo avanzado de implementación de **Backend en .NET 7**, siguiendo patrones de diseño empresariales. Se conserva como parte de un portfolio de ingeniería de software para demostrar competencias en arquitecturas desacopladas y escalables.

---

## 🎯 Visión Técnica

El proyecto implementa un sistema de gestión bancaria (Bank API) cuya prioridad es el **desacoplamiento total** entre la lógica de negocio y las dependencias de infraestructura. La arquitectura garantiza que el núcleo del sistema sea testeable, mantenible y agnóstico a la persistencia.

Principios aplicados:
*   **Separación de Responsabilidades**: Implementación estricta de las capas de Clean Architecture.
*   **CQRS (Command Query Responsibility Segregation)**: División de las operaciones de lectura y escritura para optimizar el rendimiento y la consistencia.
*   **Inversión de Dependencias**: El núcleo de la aplicación (Domain) no depende de marcos externos ni de la base de datos.

---

## 🏗️ Arquitectura de Círculos (Clean Architecture)

El código está organizado en cuatro proyectos principales:

- **1. CQRS.Domain**: El "corazón" del sistema. Contiene entidades, excepciones de dominio e interfaces de repositorio sin dependencias externas.
- **2. CQRS.Application**: Orquestación de la lógica. Define los **Commands** y **Queries**, sus **Handlers** (vía MediatR) y los DTOs. Utiliza **Mapster** para transformaciones de datos ultra-rápidas.
- **3. CQRS.Infrastructure**: Implementación de los detalles. Contiene el contexto de **Entity Framework Core**, migraciones de SQL Server y la integración con repositorios reales.
- **4. CQRS.API**: El punto de entrada REST. Controladores delgados que delegan toda la lógica a los handlers de aplicación.

---

## 🛠️ Stack Tecnológico & Patrones

*   **Runtime**: [.NET 7.0](https://dotnet.microsoft.com/download/dotnet/7.0)
*   **Mediator Pattern**: [MediatR](https://github.com/jbogard/MediatR) para el desacoplamiento de handlers.
*   **ORM**: [Entity Framework Core](https://docs.microsoft.com/ef/core/) para SQL Server.
*   **Mapping**: [Mapster](https://github.com/MapsterMapper/Mapster) - La alternativa de alto rendimiento a AutoMapper.
*   **Logging**: [Serilog](https://serilog.net/) con sumideros (sinks) para consola y **SQL Server**.
*   **Containerization**: Docker & Docker Compose para el despliegue local de la base de datos y la app.
*   **Documentation**: Swagger/OpenAPI para la exploración interactiva de la API.

---

## 🚀 Despliegue y Ejecución

El proyecto está listo para ejecutarse en contenedores, facilitando el entorno de desarrollo:

1.  **Levantar el entorno (Docker)**
    ```bash
    docker-compose up -d
    ```
2.  **Acceder a la documentación API**
    Una vez en ejecución, la interfaz de Swagger estará disponible en:
    `http://localhost:5000/swagger/index.html`

---
*Este repositorio es un testimonio de ingeniería aplicada a soluciones backend robustas y modernas.*
