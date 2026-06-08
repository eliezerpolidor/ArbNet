namespace ArbNet.Models
{
    public class ArbitrageSummary
    {
        public decimal TotalBuyFiat { get; set; }           // Total invertido en compras (Bs.)
        public decimal TotalSellFiat { get; set; }          // Total recuperado en ventas (Bs.)
        public decimal TotalVolumeCrypto { get; set; }       // Cripto total movido (USDT)
        public decimal TotalCommissionCrypto { get; set; } // Comisiones pagadas en la plataforma
        public decimal NetProfitFiat { get; set; }           // Ganancia neta real en moneda local (Bs.)
        public decimal ProfitMarginPercentage { get; set; } // Porcentaje de rendimiento (%)
        public int CompletedOrdersCount { get; set; }       // Total de órdenes exitosas
    }
}
