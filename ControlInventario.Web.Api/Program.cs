
using ControlInventario.Web.Api.Startup;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;

namespace ControlInventario.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? connectionStrings = builder.Configuration.GetSection("ConnectionStrings").GetValue<string>("BaseArticulos");

            string? secretKey = builder.Configuration.GetValue<string>("Secret");

            string? key = builder.Configuration.GetValue<string>("ApiKey");

            ControlInventario.Web.Api.Security.ApiKey.Key = string.IsNullOrEmpty(key) ? Guid.NewGuid() : new Guid(key);

            ControlInventario.Web.Api.Security.ApiKey.Secret = secretKey ?? string.Empty;

            (Action<AuthenticationOptions> authenticationOptions, Action<JwtBearerOptions> jwtBearerOptions) = ControlInventario.Web.Api.Security.Security.AddAuthentication(secretKey!);

            builder.Services.AddAuthorization()
                            .AddAuthentication(authenticationOptions)
                            .AddJwtBearer(jwtBearerOptions);

            builder.Services.AddControllers()
                            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            builder.Services.RegisterServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.ConfigureSwagger();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
