using ControlInventario.Web.Api.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using ControlInventario.Web.Api.Service;

namespace ControlInventario.Web.Api.Startup
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var cadenaConexion = configuration.GetConnectionString("BaseArticulos")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'BaseArticulos' en appsettings.json.");

            services.AddDbContext<BaseArticulosContext>(opciones =>
                opciones.UseSqlServer(cadenaConexion));

            services.AddScoped<IArticuloService, ArticuloService>();

            services.AddValidatorsFromAssemblyContaining<Program>();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Control de Inventario Web API",
                    Version = "v1"
                });

                // Evita colisiones de schema cuando existen tipos con el mismo nombre en distintos namespaces.
                options.CustomSchemaIds(type => type.FullName?.Replace("+", ".", StringComparison.Ordinal));

                // Resuelve acciones duplicadas entre controllers y minimal APIs.
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                OpenApiSecurityScheme bearerScheme = new()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT. El prefijo Bearer se agrega automáticamente."
                };

                options.AddSecurityDefinition("Bearer", bearerScheme);

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });

            return services;
        }
    }

}
