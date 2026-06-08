using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ArbNet.Models;

namespace ArbNet.Services
{
    public class BinanceP2PService
    {
        private readonly ArbNetDbContext _context;
        private readonly Random _random = new Random();

        // Variable estática para guardar las órdenes generadas actualmente
        private static List<BinanceP2POrder> _cachedOrders = new List<BinanceP2POrder>();
        private static bool _cacheInitialized = false;

        public BinanceP2PService(ArbNetDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método principal que expone tu Backend (vía Swagger o Controlador)
        /// Procesa los datos simulados de Binance, calcula la matemática y persiste en SQL Server.
        /// </summary>
        public async Task<string> ProcesarFinanzasArbNetAsync()
        {
            // Genera datos dinámicos para simular Binance
            var ordenesSimuladas = await GetSimulatedOrdersAsync();

            var ordenesParaBaseDatos = new List<BinanceP2POrder>();

            // Mapeo y Cálculos Matemáticos en Memoria
            foreach (var orden in ordenesSimuladas)
            {
                // Lógica financiera: ganancia del 1.5% en ventas
                decimal gananciaNetaCalculada = orden.TradeType == "SELL" ? (orden.TotalPrice * 0.015m) : 0m;

                // Construimos la entidad
                var nuevaEntidad = new BinanceP2POrder
                {
                    OrderNumber = orden.OrderNumber,
                    TradeType = orden.TradeType,
                    Asset = orden.Asset,
                    Fiat = orden.Fiat,
                    FiatSymbol = orden.FiatSymbol,
                    Amount = orden.Amount,
                    UnitPrice = orden.UnitPrice,
                    TotalPrice = orden.TotalPrice,
                    Commission = orden.Commission,
                    NetProfit = gananciaNetaCalculada,
                    Status = orden.Status,
                    CreateTime = orden.CreateTime,
                    PaymentMethod = orden.PaymentMethod
                };

                ordenesParaBaseDatos.Add(nuevaEntidad);
            }

            // Guardado en SQL Server
            await _context.BinanceP2POrders.AddRangeAsync(ordenesParaBaseDatos);
            await _context.SaveChangesAsync();

            return $"¡Éxito! Se procesaron {ordenesParaBaseDatos.Count} órdenes de Binance en memoria, se calcularon las finanzas y se persistieron correctamente en SQL Server.";
        }

        /// <summary>
        /// Obtiene el historial de órdenes P2P desde la base de datos o datos simulados DINÁMICOS
        /// </summary>
        public async Task<List<BinanceP2POrder>> GetOrderHistoryAsync(bool useTestnet)
        {
            if (useTestnet)
            {
                // Siempre genera nuevas órdenes cuando se llama al historial
                _cachedOrders = await GetSimulatedOrdersAsync();
                _cacheInitialized = true;
                return _cachedOrders;
            }
            else
            {
                // Modo producción: obtener desde la base de datos
                return await _context.BinanceP2POrders
                    .Where(o => o.Status == "COMPLETED")
                    .OrderByDescending(o => o.CreateTime)
                    .ToListAsync();
            }
        }

        /// <summary>
        /// Genera una lista de datos simulados dinámicos que cambian en cada llamado (datos Aleatorios)
        /// </summary>
        private async Task<List<BinanceP2POrder>> GetSimulatedOrdersAsync()
        {
            // Simular latencia de red realista
            await Task.Delay(150);

            var ordersList = new List<BinanceP2POrder>();

            // Bancos típicos para simular el arbitraje local
            var bancos = new[] { "Pago Móvil", "Banesco", "Mercantil", "Zinli" };

            // Base de tiempos: Empezamos hace unas horas atrás para simular un historial continuo
            var tiempoBase = DateTimeOffset.UtcNow.AddHours(-5);

            for (int i = 0; i < 10; i++)
            {
                // Alternamos compras y ventas para poder calcular spreads en el frontend
                string tradeType = (i % 2 == 0) ? "BUY" : "SELL";

                // Forzamos un spread lógico: Compras un poco más barato de lo que vendes
                decimal precioBase = 40.50m;
                decimal precioSimulado = tradeType == "BUY"
                    ? precioBase - (decimal)(_random.NextDouble() * 0.25)
                    : precioBase + (decimal)(_random.NextDouble() * 0.45);

                // Volúmenes variados para transacciones realistas
                var volumenes = new[] { 50.00m, 120.00m, 350.00m, 800.00m, 1150.00m };
                decimal cantidadSimulada = volumenes[_random.Next(volumenes.Length)];
                decimal total = Math.Round(cantidadSimulada * precioSimulado, 2);

                // Comisión estándar de anunciante P2P de Binance (ej: 0.1% o un fijo pequeño en crypto)
                decimal comisionSimulada = Math.Round(cantidadSimulada * 0.001m, 4);

                // Incrementamos el tiempo de cada orden consecutivas para que tengan orden cronológico
                tiempoBase = tiempoBase.AddMinutes(_random.Next(20, 50));

                ordersList.Add(new BinanceP2POrder
                {
                    OrderNumber = "50" + _random.Next(100000, 999999) + _random.Next(100000, 999999),
                    AdvNo = "11" + _random.Next(100000, 999999),
                    TradeType = tradeType,
                    Asset = "USDT",
                    Fiat = "VES",
                    FiatSymbol = "Bs.",
                    Amount = cantidadSimulada,
                    UnitPrice = precioSimulado,
                    TotalPrice = total,
                    Commission = comisionSimulada,
                    Status = _random.Next(0, 10) == 0 ? "CANCELLED" : "COMPLETED",
                    CreateTime = tiempoBase.ToUnixTimeMilliseconds(),
                    PaymentMethod = bancos[_random.Next(bancos.Length)],
                    NetProfit = tradeType == "SELL" ? (total * 0.015m) : 0m
                });
            }

            // Invertimos la lista para que la orden más reciente salga de primera, ideal para UI
            ordersList.Reverse();
            return ordersList;
        }

        /// <summary>
        /// Calcula el resumen de arbitraje - usa las mismas órdenes guardadas en caché
        /// </summary>
        public async Task<ArbitrageSummary> GetArbitrageCalculationsAsync(bool useTestnet)
        {
            List<BinanceP2POrder> ordenes;

            if (useTestnet)
            {
                // USA las órdenes que ya se generaron en GetOrderHistoryAsync
                // Si no hay caché todavía, genera nuevas
                if (!_cacheInitialized || _cachedOrders.Count == 0)
                {
                    _cachedOrders = await GetSimulatedOrdersAsync();
                }
                ordenes = _cachedOrders;
            }
            else
            {
                // Modo producción: obtener desde la base de datos
                ordenes = await _context.BinanceP2POrders
                    .Where(o => o.Status == "COMPLETED")
                    .ToListAsync();
            }

            // Si no hay órdenes, retorna valores por defecto
            if (ordenes == null || ordenes.Count == 0)
            {
                return new ArbitrageSummary
                {
                    TotalBuyFiat = 0,
                    TotalSellFiat = 0,
                    TotalVolumeCrypto = 0,
                    TotalCommissionCrypto = 0,
                    NetProfitFiat = 0,
                    ProfitMarginPercentage = 0,
                    CompletedOrdersCount = 0
                };
            }

            // Filtra solo las órdenes completadas para los cálculos
            var ordenesCompletadas = ordenes.Where(o => o.Status == "COMPLETED").ToList();

            // Calcula los totales
            decimal totalBuyFiat = ordenesCompletadas
                .Where(o => o.TradeType?.ToUpper() == "BUY")
                .Sum(o => o.TotalPrice);

            decimal totalSellFiat = ordenesCompletadas
                .Where(o => o.TradeType?.ToUpper() == "SELL")
                .Sum(o => o.TotalPrice);

            decimal totalVolumeCrypto = ordenesCompletadas.Sum(o => o.Amount);
            decimal totalCommissionCrypto = ordenesCompletadas.Sum(o => o.Commission);
            decimal netProfitFiat = totalSellFiat - totalBuyFiat - totalCommissionCrypto;

            // Calcula el porcentaje de ganancia
            decimal profitMargin = totalBuyFiat > 0
                ? (netProfitFiat / totalBuyFiat) * 100
                : 0;

            return new ArbitrageSummary
            {
                TotalBuyFiat = totalBuyFiat,
                TotalSellFiat = totalSellFiat,
                TotalVolumeCrypto = totalVolumeCrypto,
                TotalCommissionCrypto = totalCommissionCrypto,
                NetProfitFiat = netProfitFiat,
                ProfitMarginPercentage = Math.Round(profitMargin, 2),
                CompletedOrdersCount = ordenesCompletadas.Count
            };
        }
    }
}