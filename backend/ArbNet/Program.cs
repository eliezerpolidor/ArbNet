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
                    string postgresConnectionString;

                    // 1. En Railway, las variables nativas de Postgres siempre están disponibles directamente
                    var pgHost = Environment.GetEnvironmentVariable("PGHOST");

                    if (!string.IsNullOrEmpty(pgHost))
                    {
                        var pgPort = Environment.GetEnvironmentVariable("PGPORT");
                        var pgDatabase = Environment.GetEnvironmentVariable("PGDATABASE");
                        var pgUser = Environment.GetEnvironmentVariable("PGUSER");
                        var pgPassword = Environment.GetEnvironmentVariable("PGPASSWORD");

                        postgresConnectionString = $"Host={pgHost};Port={pgPort};Database={pgDatabase};Username={pgUser};Password={pgPassword};";
                    }
                    else
                    {
                        // 2. Si no está en Railway (entorno local), usa el appsettings.json estándar
                        postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");
                    }

                    options.UseNpgsql(postgresConnectionString);
                }
                //if (dbProvider?.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) == true)
                //{
                //    // .NET automáticamente convierte "ConnectionStrings__PostgresConnection" de Railway
                //    // en una clave accesible mediante GetConnectionString("PostgresConnection")
                //    var postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");

                //    options.UseNpgsql(postgresConnectionString);
                //}
                //if (dbProvider?.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) == true)
                //{
                //    // 1. Intenta leer directamente la variable tal como está escrita en Railway
                //    var postgresConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection");

                //    // 2. Si no la encuentra (porque estás en local), usa la configuración tradicional
                //    if (string.IsNullOrEmpty(postgresConnectionString))
                //    {
                //        postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");
                //    }

                //    options.UseNpgsql(postgresConnectionString);
                //}
            });
            // ===================================================================

            var app = builder.Build();

            // Bloque de inicialización del Contexto
            //using (var scope = app.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetRequiredService<ArbNetDbContext>();
            //    // context.Database.EnsureCreated(); // Mantener comentado para no chocar con las tablas creadas manualmente
            //}
            // Asegúrate de que este bloque exista antes de 'app.Run();'
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ArbNetDbContext>();
                    // Esta línea mágica revisa si las tablas existen; si no, las crea al instante
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Un error ocurrió al crear la base de datos.");
                }
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
