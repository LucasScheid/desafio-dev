using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace cnab_api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddCors();
            services.AddControllers();            
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

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
