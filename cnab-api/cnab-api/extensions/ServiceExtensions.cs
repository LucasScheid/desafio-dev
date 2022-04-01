using cnab_contracts.database;
using cnab_entities.map;
using cnab_entities.models;
using cnab_helpers.database;
using cnab_helpers.environment;
using cnab_infra.database;
using cnab_infra.database.data;
using cnab_services.arquivo;
using cnab_services.database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using System.Text;

namespace cnab_api.extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
           services.AddCors(options =>
           {
               options.AddPolicy("CorsPolicy", builder =>
                   builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());
           });

        private static string GetDBConnectionString()
        {
            StringBuilder sbConnectionString = new();

            sbConnectionString.Append("server=");
            sbConnectionString.Append(Environment.GetEnvironmentVariable("SQLSERVER_HOST"));
            sbConnectionString.Append(';');
            sbConnectionString.Append("database=");
            sbConnectionString.Append(Environment.GetEnvironmentVariable("SQLSERVER_DATABASE"));
            sbConnectionString.Append(';');
            sbConnectionString.Append("user id=");
            sbConnectionString.Append(Environment.GetEnvironmentVariable("SQLSERVER_USER"));
            sbConnectionString.Append(';');
            sbConnectionString.Append("password=");
            sbConnectionString.Append(Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD"));
            sbConnectionString.Append(';');
            sbConnectionString.Append(Environment.GetEnvironmentVariable("SQLSERVER_ADDITIONAL_CONFIGS"));

            return sbConnectionString.ToString();
        }

        public static void ConfigureSqlServerConnection(this IServiceCollection services)
        {
            services.AddScoped<IDBConnection, SqlServerConnection>(_ => new SqlServerConnection(new SqlConnection(GetDBConnectionString())));
        }

        public static void ConfigurarBehaviorOptions(this IServiceCollection services) =>
          services.Configure<ApiBehaviorOptions>(options =>
          {
              options.SuppressModelStateInvalidFilter = true;
          });

        public static void ConfigurarSwagger(this IServiceCollection services) =>
       services.AddSwaggerGen(c =>
       {
           c.SwaggerDoc(Environment.GetEnvironmentVariable("API_VERSION"), new OpenApiInfo
           {
               Title = Environment.GetEnvironmentVariable("APPLICATION_NAME"),
               Version = Environment.GetEnvironmentVariable("API_VERSION"),
               Description = Environment.GetEnvironmentVariable("APPLICATION_DESCRIPTION"),
           });

           c.EnableAnnotations();

           var securityScheme = new OpenApiSecurityScheme
           {
               Name = "Autenticacao JWT",
               Description = "Informe o token JWT Bearer **_somente_**",
               In = ParameterLocation.Header,
               Type = SecuritySchemeType.Http,
               Scheme = "bearer",
               BearerFormat = "JWT",
               Reference = new OpenApiReference
               {
                   Id = JwtBearerDefaults.AuthenticationScheme,
                   Type = ReferenceType.SecurityScheme
               }
           };

           c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

           c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>()}
                });

       });

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IEnvironmentHelper, EnvironmentHelper>();
            services.AddScoped<ISqlCommandHelper, SqlCommandHelper>();
            services.AddScoped<ISqlOperationHelper, SqlOperationHelper>();

            services.AddScoped<IArquivoSqlCommand, ArquivoSqlCommand>();
            services.AddScoped<IArquivoDbService, ArquivoDbService>();
            services.AddScoped<ITipoTransacaoSqlCommand, TipoTransacaoSqlCommand>();
            services.AddScoped<ITipoTransacaoDbService, TipoTransacaoDbService>();

            services.AddTransient<IArquivoService<CNAB>, ArquivoService>();
            services.AddTransient<IArquivoMapPosicao, MapCNABTXT>();
        }

    }
}
