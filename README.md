# API Control de Inventario - Artículos

Proyecto final del curso **Fundamentos de .NET con C#**.  
Consiste en una Web API REST para gestionar la tabla `[Adquisiciones].[Articulo]` de la base de datos `BaseArticulos` en SQL Server, con autenticación JWT y documentación OpenAPI (Swagger).

## Tecnologías utilizadas

- .NET 10 / ASP.NET Core Web API (controladores)
- Entity Framework Core 10 + LINQ + SQL Server
- Autenticación basada en API Key y JWT Bearer (`[Authorize]`)
- OpenAPI / Swagger (`Swashbuckle.AspNetCore`)
- Data Annotations y validación de integridad en capa de servicios
- Logging con `ILogger`

## Estructura del proyecto

```text
ControlInventario.Web.Api/
├── Controllers/
│   ├── ApiKey/ApiKeyController.cs
│   ├── Test/TestController.cs
│   └── ArticulosController.cs
├── Data/
│   └── BaseArticulosContext.cs
├── Dto/
│   ├── ArticuloDto.cs
│   └── ArticuloRespuestaDto.cs
├── Model/
│   ├── Empresa.cs
│   ├── Familia.cs
│   ├── SubFamilia.cs
│   └── Articulo.cs
├── Security/
├── Service/
│   ├── IArticuloService.cs
│   └── ArticuloService.cs
└── Startup/
    ├── DependencyInjectionConfiguration.cs
    └── SwaggerConfiguration.cs
```

## Requisitos previos

1. Tener instalado **.NET 10 SDK** y **SQL Server**.
2. Crear y poblar la base de datos `BaseArticulos` ejecutando en orden:
   - `1_BaseArticulosEstructura.sql`
   - `2_BaseArticulosDatos.sql`
3. Ajustar la cadena de conexión `BaseArticulos` en `ControlInventario.Web.Api/appsettings.json` si tu instancia local de SQL Server es diferente a `(local)`.

## Cómo ejecutar y probar en Swagger

1. Abrir `ControlInventario.slnx` en Visual Studio y ejecutar con el perfil `http` (o usar `dotnet run --project ControlInventario.Web.Api`).
2. Abrir en el navegador `http://localhost:5000/swagger`.
3. Ejecutar `GET /api/ApiKey` enviando la `ApiKey` configurada en `appsettings.json` para obtener el token JWT.
4. Copiar el token devuelto en `"data"`, hacer clic en **Authorize** en Swagger y pegarlo.

## Endpoints principales (`/api/Articulos`)

Todos los endpoints de artículos requieren token JWT (`[Authorize]`):

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/Articulos` | Lista todos los artículos mostrando el nombre de su empresa, familia y subfamilia |
| `GET` | `/api/Articulos/{id}` | Obtiene un artículo por su `Guid` |
| `POST` | `/api/Articulos` | Registra un artículo nuevo validando campos y existencia de empresa y subfamilia |
| `PUT` | `/api/Articulos/{id}` | Actualiza los datos de un artículo existente |
| `DELETE` | `/api/Articulos/{id}` | Elimina un artículo individualmente |
