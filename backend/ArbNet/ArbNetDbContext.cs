using Microsoft.EntityFrameworkCore;
using ArbNet.Models;

namespace ArbNet
{
    public class ArbNetDbContext : DbContext
    {
        public ArbNetDbContext(DbContextOptions<ArbNetDbContext> options) : base(options)
        {
        }

        // Esta propiedad le dice a EF Core que cree la tabla en SQL Server basada en tu modelo
        public DbSet<BinanceP2POrder> BinanceP2POrders { get; set; }

        public DbSet<User> Users { get; set; }
        // Si necesitas persistir los resúmenes financieros más adelante, descomenta esta línea:
        // public DbSet<ArbitrageSummary> ArbitrageSummaries { get; set; }
    }
}