using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestAoniken.Data;
using TestAoniken.Servicios;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

public class Startup
{
    public IConfiguration Configuration { get; }

    // Constructor que recibe la configuraci�n del archivo appsettings.json
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // Configuraci�n de los servicios necesarios para la aplicaci�n
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Configurar DbContext con SQL Server utilizando Entity Framework Core
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Configurar servicios personalizados
        services.AddScoped<IPublicacionService, PublicacionService>();

        // Configurar Swagger para la documentaci�n de la API
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestAoniken API", Version = "v1" });
        });
    }

    // Configuraci�n del pipeline de la aplicaci�n
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configuraci�n para el entorno de desarrollo
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // Configuraci�n para el entorno de producci�n
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Forzar redirecci�n HTTPS
        app.UseHttpsRedirection();

        // Habilitar archivos est�ticos
        app.UseStaticFiles();

        // Configurar enrutamiento
        app.UseRouting();

        // Habilitar autorizaci�n
        app.UseAuthorization();

        // Habilitar Swagger y Swagger UI para la documentaci�n de la API
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestAoniken API V4");
            c.RoutePrefix = string.Empty; // Configurar Swagger en la ruta ra�z
        });

        // Configurar los endpoints para los controladores
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
