using cnab_api.extensions;

namespace cnab_api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureServices();
            services.ConfigureSqlServerConnection();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{Environment.GetEnvironmentVariable("API_VERSION")}/swagger.json",
                    $"{Environment.GetEnvironmentVariable("APPLICATION_NAME")} - {Environment.GetEnvironmentVariable("API_VERSION")}");
            });

        }
    }
}
