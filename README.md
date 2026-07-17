# SGE - Sistema de Gestión de Expedientes

Sistema web para centralizar la gestión de expedientes administrativos, sus carátulas, estados y trámites asociados. El objetivo es reemplazar el seguimiento disperso de documentación por un flujo digital trazable, con usuarios autenticados, permisos diferenciados y una interfaz preparada para el trabajo diario.

El proyecto está desarrollado como una solución .NET separada en capas, con una WebAPI REST y un frontend administrativo en Blazor WebAssembly.

## Funcionalidades implementadas

- Registro e inicio de sesión con autenticación JWT.
- Persistencia de sesión en el frontend y envío automático del token Bearer.
- Dashboard administrativo con métricas y expedientes recientes.
- Alta, consulta, modificación y baja de expedientes.
- Edición de carátulas y actualización manual de estados.
- Consulta de trámites dentro del detalle de cada expediente.
- Alta, modificación y baja de trámites.
- Etiquetas de trámite que pueden actualizar el estado del expediente.
- Gestión de usuarios para cuentas administradoras.
- Administración de permisos por usuario.
- Edición de los datos personales y contraseña propia.
- Rutas protegidas en la interfaz y respuestas diferenciadas para `401`, `403` y `404`.
- Estados de carga, estados vacíos, errores inline y diálogos de confirmación.
- Manejo global de errores mediante `ProblemDetails` y `traceId`.
- Endpoint `/health` para comprobar el estado de la aplicación y la base de datos.
- Rate limiting para reducir intentos abusivos sobre el login.
- Configuración de CORS, JWT y URL de la API por entorno.

## Arquitectura

La solución utiliza una arquitectura por capas con responsabilidades separadas:

```text
SGE.Frontend       Interfaz administrativa Blazor WebAssembly
       |
SGE.WebAPI         Endpoints HTTP, autenticación, CORS y middleware
       |
SGE.Aplicacion     Casos de uso, contratos, DTOs e interfaces
       |
SGE.Dominio        Entidades, enums, reglas y objetos de valor
       |
SGE.Infraestructura Repositorios, Entity Framework Core, SQLite y servicios técnicos
```

### Proyectos principales

- `SGE.Dominio`: contiene las entidades `Expediente`, `Tramite` y `Usuario`, además de objetos de valor como `Caratula` y `ContenidoTramite`.
- `SGE.Aplicacion`: concentra los casos de uso y contratos de entrada y salida. Esta capa no depende de la presentación ni de la base de datos.
- `SGE.Infraestructura`: implementa repositorios, unidad de trabajo, persistencia con Entity Framework Core y servicios de hash y autorización.
- `SGE.WebAPI`: expone los endpoints REST, configura la inyección de dependencias, JWT, CORS, rate limiting, health checks y manejo global de excepciones.
- `SGE.Frontend`: aplicación Blazor WebAssembly con layout administrativo, navegación protegida, formularios, tablas y consumo tipado de la API.

## Conceptos informáticos utilizados

### Arquitectura y diseño

- Arquitectura por capas para separar dominio, aplicación, infraestructura y presentación.
- Separación de responsabilidades para reducir el acoplamiento entre reglas de negocio, persistencia y UI.
- Casos de uso para representar operaciones concretas como crear un expediente, modificar una carátula o agregar un trámite.
- Patrón Repository para abstraer el acceso a los datos.
- Unidad de Trabajo para confirmar los cambios de una operación de negocio.
- Inyección de dependencias para registrar e intercambiar implementaciones sin acoplar los casos de uso a detalles técnicos.

### Modelado de dominio

- Entidades con identidad propia y reglas encapsuladas.
- Objetos de valor para representar conceptos que deben validarse, como carátulas y contenidos.
- Enumeraciones para estados, etiquetas y permisos.
- Reglas de autorización asociadas a operaciones administrativas.
- Auditoría básica mediante fechas y usuario del último cambio.

### Backend y APIs

- API REST construida con ASP.NET Core Minimal APIs.
- Métodos HTTP y códigos de estado como `200`, `201`, `204`, `400`, `401`, `403`, `404` y `500`.
- DTOs para evitar exponer directamente las entidades del dominio.
- JWT Bearer para autenticación sin estado.
- Claims para identificar al usuario autenticado.
- Autorización basada en administrador y permisos específicos.
- `ProblemDetails` para mantener respuestas de error consistentes.
- Scalar/OpenAPI para explorar y documentar la API durante el desarrollo.

### Persistencia y seguridad

- Entity Framework Core como ORM.
- SQLite para desarrollo y demostración local.
- Índice único para evitar correos duplicados.
- Hashing de contraseñas con PBKDF2-SHA256 y salt individual.
- Compatibilidad temporal para actualizar hashes heredados al iniciar sesión.
- CORS configurable por entorno.
- Rate limiting para limitar intentos de autenticación.
- Health checks para detectar problemas de disponibilidad o conexión con la base de datos.

## Tecnologías y herramientas

- C#
- .NET 10
- ASP.NET Core Minimal APIs
- Blazor WebAssembly
- Entity Framework Core
- SQLite
- JWT Bearer Authentication
- Scalar/OpenAPI
- HTML y CSS
- .NET CLI
- Git y GitHub

## Cómo fue desarrollado

1. Se analizó el dominio de una mesa de entradas digital y se identificaron sus conceptos principales: expedientes, carátulas, trámites, estados, usuarios y permisos.
2. Se modelaron las entidades y objetos de valor en `SGE.Dominio`, encapsulando validaciones y reglas de negocio.
3. Se implementaron casos de uso independientes de la base de datos en `SGE.Aplicacion`.
4. Se definieron interfaces para repositorios, autorización, hash de contraseñas y unidad de trabajo.
5. Se implementaron esas interfaces en `SGE.Infraestructura` utilizando Entity Framework Core y SQLite.
6. Se construyeron endpoints REST en `SGE.WebAPI` para autenticación, usuarios, expedientes y trámites.
7. Se agregó autenticación JWT y autorización basada en roles y permisos.
8. Se incorporó manejo global de excepciones, configuración por entorno, rate limiting y health checks.
9. Se desarrolló una interfaz administrativa propia en Blazor WebAssembly, conectada a la API mediante servicios HTTP reutilizables.
10. Se diseñó la interfaz visual alrededor del concepto de una mesa de entradas digital, con navegación tipo archivo, tablas administrativas y una línea temporal de actuaciones.

## Cómo ejecutar en local

Requisitos:

- .NET SDK 10.
- Git, si se clona el repositorio.

Desde la carpeta que contiene `SGE.slnx`:

```bash
dotnet build SGE.slnx
dotnet run --project SGE.WebAPI --launch-profile http
dotnet run --project SGE.Frontend --launch-profile http
```

URLs locales:

- Frontend: `http://localhost:5104`
- API: `http://localhost:5134`
- Health check: `http://localhost:5134/health`
- Documentación OpenAPI/Scalar: disponible en el entorno de desarrollo de la API.

La API y el frontend se ejecutan como procesos separados durante el desarrollo.

## Despliegue del frontend en Vercel

El repositorio incluye `vercel.json` para publicar automáticamente el frontend
Blazor WebAssembly y resolver correctamente sus rutas internas al recargar la
página.

En Vercel, importar el repositorio usando como raíz la carpeta que contiene
`vercel.json` y crear la variable de entorno `API_BASE_URL` con la URL pública
de la API, por ejemplo `https://api.ejemplo.com`. Luego ejecutar un nuevo deploy.

La API (`SGE.WebAPI`) debe desplegarse como un servicio .NET separado. No se
debe usar `localhost` en `API_BASE_URL`, porque el navegador del usuario no
puede acceder al servidor local del desarrollador. También hay que agregar la
URL pública de Vercel a `Cors__AllowedOrigins__0` en la configuración de la API.

## Configuración por entorno

En producción, los valores sensibles deben configurarse mediante variables de entorno o un gestor de secretos. No se debe versionar una clave JWT real.

```text
ConnectionStrings__SgeDb
Jwt__Key
Jwt__Issuer
Jwt__Audience
Jwt__ExpirationMinutes
Cors__AllowedOrigins__0
```

`Jwt__Key` debe tener al menos 32 caracteres. El origen permitido del frontend debe declararse explícitamente en `Cors__AllowedOrigins__0`.

## Usuarios demo para desarrollo

La aplicación crea estas cuentas sólo si no existen en la base local:

| Perfil | Usuario | Contraseña |
|---|---|---|
| Administrador | `admin@sge.com` | `admin123` |
| Operador | `operador@sge.com` | `operador123` |
| Consulta | `lector@sge.com` | `lector123` |

Son credenciales de demostración. Deben eliminarse, cambiarse o deshabilitarse antes de cualquier instalación real.

## Estado actual y roadmap

El sistema cuenta con una base funcional para demostración, portfolio y evolución hacia un producto. Antes de comercializarlo y utilizarlo con expedientes reales todavía sería necesario:

- Migrar SQLite a PostgreSQL o SQL Server administrado.
- Incorporar migraciones EF Core versionadas.
- Definir multi-tenancy para separar organizaciones y datos.
- Implementar recuperación de contraseña y verificación de correo.
- Agregar autenticación multifactor y sesiones revocables.
- Crear una auditoría inmutable de operaciones y cambios.
- Implementar backups, restauración, retención y exportación de datos.
- Incorporar pruebas unitarias, de integración, de API y de navegador.
- Agregar monitoreo, logs centralizados y alertas operativas.
- Definir límites por plan, facturación y gestión de suscripciones.
- Preparar política de privacidad, términos de servicio, SLA y soporte.
- Realizar análisis de dependencias, SAST, DAST, pruebas de carga y pentest.

## Descripción para portfolio

### SGE - Sistema de Gestión de Expedientes

**Rol:** Software Developer

Desarrollé un sistema web para la gestión digital de expedientes administrativos, carátulas, estados y trámites. El objetivo del proyecto es centralizar la información documental y facilitar el seguimiento de cada actuación desde una interfaz administrativa clara y orientada al trabajo diario.

Implementé autenticación con JWT, autorización por roles y permisos, operaciones CRUD para expedientes y trámites, administración de usuarios, manejo global de errores y controles básicos de seguridad. También desarrollé un dashboard administrativo en Blazor WebAssembly conectado a una WebAPI REST.

La solución está organizada en capas de dominio, aplicación, infraestructura y presentación. Utilicé entidades y objetos de valor para modelar el negocio, casos de uso para encapsular operaciones, repositorios y unidad de trabajo para la persistencia, e inyección de dependencias para mantener los componentes desacoplados.

**Tecnologías:** C#, .NET 10, ASP.NET Core Minimal APIs, Blazor WebAssembly, Entity Framework Core, SQLite, JWT, Scalar/OpenAPI, HTML, CSS, .NET CLI, Git y GitHub.

Este proyecto me permitió profundizar en arquitectura de software, diseño de APIs, modelado de dominio, autenticación, autorización, persistencia relacional, diseño de interfaces administrativas y evolución de un prototipo hacia una base de producto escalable.

**Enlaces:**

- GitHub: `[agregar enlace]`
- Demo: `[agregar enlace]`
- Portfolio: `[agregar enlace]`
- Video de presentación: `[agregar enlace]`

## Autor

Valentín Lolla Galván

- LinkedIn: `www.linkedin.com/in/valentin-lolla-galvan`
- Email: `valentinlollagalvan@gmail.com`
