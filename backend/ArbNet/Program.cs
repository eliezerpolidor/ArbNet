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

                    // Si la aplicación está corriendo localmente en tu computadora
                    if (builder.Environment.IsDevelopment())
                    {
                        postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");
                    }
                    // Si la aplicación está corriendo en producción dentro de Railway
                    else
                    {
                        // Railway inyecta de forma obligatoria y directa la variable "DATABASE_URL"
                        // que contiene el Host, Usuario, Puerto y Contraseña interna actualizados al segundo.
                        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                        if (string.IsNullOrEmpty(databaseUrl))
                        {
                            // Respaldo por si acaso
                            postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");
                        }
                        else
                        {
                            // Convertimos el formato postgresql://usuario:clave@host:puerto/db al formato que entiende EF Core
                            var databaseUri = new Uri(databaseUrl);
                            var userInfo = databaseUri.UserInfo.Split(':');

                            postgresConnectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};Include Error Detail=true;";
                        }
                    }

                    options.UseNpgsql(postgresConnectionString);
                }
                //if (dbProvider?.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) == true)
                //{
                //    var postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");

                //    // Si la cadena viene de Railway como una URL (empieza con postgresql:// o postgres://)
                //    if (!string.IsNullOrEmpty(postgresConnectionString) &&
                //        (postgresConnectionString.StartsWith("postgres://") || postgresConnectionString.StartsWith("postgresql://")))
                //    {
                //        // Parseamos la URL para convertirla al formato "Host=...;Port=..." que entiende EF Core
                //        var databaseUri = new Uri(postgresConnectionString);
                //        var userInfo = databaseUri.UserInfo.Split(':');

                //        postgresConnectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true;";
                //    }

                //    options.UseNpgsql(postgresConnectionString);
                //}
                //if (dbProvider?.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) == true)
                //{
                //    // 1. Intentamos leer la variable de Railway
                //    var postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");

                //    // 2. Si por algún motivo Railway no la inyecta bien y llega vacía o sin el "Host",
                //    // le forzamos la cadena de texto real fija de producción directamente en el código.
                //    if (string.IsNullOrEmpty(postgresConnectionString) || !postgresConnectionString.Contains("Host="))
                //    {
                //        postgresConnectionString = "Host=thomas.proxy.rlwy.net;Port=22337;Database=railway;Username=postgres;Password=gowaAFKYwsprvoeskPzqdHfPtSHfkjDY;";
                //    }

                //    options.UseNpgsql(postgresConnectionString);
                //}
                //if (dbProvider?.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) == true)
                //{
                //    string postgresConnectionString;

                //    // 1. En Railway, las variables nativas de Postgres siempre están disponibles directamente
                //    var pgHost = Environment.GetEnvironmentVariable("PGHOST");

                //    if (!string.IsNullOrEmpty(pgHost))
                //    {
                //        var pgPort = Environment.GetEnvironmentVariable("PGPORT");
                //        var pgDatabase = Environment.GetEnvironmentVariable("PGDATABASE");
                //        var pgUser = Environment.GetEnvironmentVariable("PGUSER");
                //        var pgPassword = Environment.GetEnvironmentVariable("PGPASSWORD");

                //        postgresConnectionString = $"Host={pgHost};Port={pgPort};Database={pgDatabase};Username={pgUser};Password={pgPassword};";
                //    }
                //    else
                //    {
                //        // 2. Si no está en Railway (entorno local), usa el appsettings.json estándar
                //        postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");
                //    }

                //    options.UseNpgsql(postgresConnectionString);
                //}
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
