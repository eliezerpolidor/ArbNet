using System.Text.Json.Serialization;

namespace ArbNet.Models.DTOs
{
    public class BinanceRawOrderDto
    {
        [JsonPropertyName("orderNo")]
        public string OrderNo { get; set; } = string.Empty;

        [JsonPropertyName("advType")]
        public string AdvType { get; set; } = string.Empty; // "BUY" o "SELL"

        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty; // Ej. "USDT"

        [JsonPropertyName("fiat")]
        public string Fiat { get; set; } = string.Empty; // Ej. "VES"

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("createTime")]
        public long CreateTime { get; set; } // Timestamp de Binance
    }
}