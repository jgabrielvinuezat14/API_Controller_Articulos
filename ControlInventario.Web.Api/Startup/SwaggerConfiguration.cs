namespace ControlInventario.Web.Api.Startup
{
    public static class SwaggerConfiguration
    {
        public static WebApplication ConfigureSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Control de Inventario Web API v1");
                options.RoutePrefix = "swagger";
                options.EnablePersistAuthorization();
            });

            return app;
        }
    }
}