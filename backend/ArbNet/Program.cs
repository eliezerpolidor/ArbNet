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

            // 1. DEFINIR LA POLÍTICA DE CORS (Agrégalo aquí)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirReact", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5115") // Puertos comunes de React/Vite
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

            // Reemplaza "TuCadenaDeConexionAqui" por tu Server, Database, etc., de SQL Server
            builder.Services.AddDbContext<ArbNetDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // 2. REQUERIDO: Habilitar el Middleware de CORS antes de la autorización
            app.UseCors("PermitirReact");

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
