using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArbNet.Models
{
    [Table("BinanceP2POrders")]
    public class BinanceP2POrder
    {
        /// <summary>
        /// Número único de la orden
        /// </summary>
        [Key]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// Número del anunciante
        /// </summary>
        public string AdvNo { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de operación: BUY (compra) o SELL (venta)
        /// </summary>
        public string TradeType { get; set; } = string.Empty;

        /// <summary>
        /// Criptomoneda (USDT, BTC, ETH)
        /// </summary>
        public string Asset { get; set; } = string.Empty;

        /// <summary>
        /// Moneda Fiat (VES, COP, USD)
        /// </summary>
        public string Fiat { get; set; } = string.Empty;

        /// <summary>
        /// Símbolo de la moneda local (Bs., $)
        /// </summary>
        public string FiatSymbol { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de criptomoneda
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Precio por unidad
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Precio total
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Comisión de Binance
        /// </summary>
        public decimal Commission { get; set; }

        /// <summary>
        /// Estado de la orden (COMPLETED, CANCELLED)
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp de creación
        /// </summary>
        public long CreateTime { get; set; }

        //-------------------------------------
        //add new eliezer
        //------------------------------------

        /// <summary>
        /// Método de pago utilizado (Ej: Pago Móvil, Banesco, Zinli)
        /// </summary>
        public string PaymentMethod { get; set; } = string.Empty;

        /// <summary>
        /// Ganancia neta calculada del arbitraje
        /// </summary>
        public decimal NetProfit { get; set; }

        /// <summary>
        /// Propiedad calculada: Fecha y hora legible a partir del Timestamp
        /// </summary>
        public DateTime FormattedCreateTime => DateTimeOffset.FromUnixTimeMilliseconds(CreateTime).DateTime.ToLocalTime();

        /// <summary>
        /// Propiedad calculada: El dinero real neto (descontando o sumando la comisión según la operación)
        /// </summary>
        public decimal NetAmount
        {
            get
            {
                return TradeType?.ToUpper() == "BUY" ? Amount - Commission : Amount;
            }
        }
    }
}
