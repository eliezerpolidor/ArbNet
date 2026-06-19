using System.IO;
using ArbNet.Services;
using Microsoft.EntityFrameworkCore;

namespace ArbNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. DEFINIR LA POLÍTICA DE CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirReact", policy =>
                {
                    policy.WithOrigins(
                        "https://empowering-gentleness-production-33bc.up.railway.app", // Tu frontend en Railway
                        "http://localhost:5173",                                        // Tu local con Vite / React
                        "http://localhost:3000"                                         // Por si usas Create-React-App local
                     )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddScoped<BinanceP2PService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"ArbNet.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddBinance();

            // ===================================================================
            // CONFIGURACIÓN DINÁMICA DE LA BASE DE DATOS (Manejador de 3 Vías)
            // ===================================================================
            var dbProvider = builder.Configuration["DatabaseSettings:UseProvider"];

            builder.Services.AddDbContext<ArbNetDbContext>(options =>
            {
                if (dbProvider?.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) == true)
                {
                    var postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");
                    options.UseNpgsql(postgresConnectionString);
                }
                else if (dbProvider?.Equals("LocalSqlServer", StringComparison.OrdinalIgnoreCase) == true)
                {
                    var localConnectionString = builder.Configuration.GetConnectionString("LocalSqlServerConnection");
                    options.UseSqlServer(localConnectionString);
                }
                else if (dbProvider?.Equals("AzureSqlServer", StringComparison.OrdinalIgnoreCase) == true)
                {
                    var azureConnectionString = builder.Configuration.GetConnectionString("AzureSqlServerConnection");
                    options.UseSqlServer(azureConnectionString);
                }
                else
                {
                    throw new InvalidOperationException($"El proveedor de base de datos configurado '{dbProvider}' no es válido.");
                }
            });
            // ===================================================================

            var app = builder.Build();

            // Bloque de inicialización del Contexto
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ArbNetDbContext>();
                // context.Database.EnsureCreated(); // Mantener comentado para no chocar con las tablas creadas manualmente
            }

            // Permitir Swagger tanto en desarrollo local como en producción de Railway
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArbNet API v1");
            });

            // 2. Middleware de CORS antes de la autorización
            app.UseCors("PermitirReact");

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
