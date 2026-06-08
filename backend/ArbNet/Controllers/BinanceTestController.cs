using Microsoft.AspNetCore.Mvc;
using Binance.Net.Interfaces.Clients;

namespace ArbNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BinanceTestController : ControllerBase
    {
        private readonly IBinanceRestClient _binanceRestClient;

        // Inyectamos el cliente REST de Binance
        public BinanceTestController(IBinanceRestClient binanceRestClient)
        {
            _binanceRestClient = binanceRestClient;
        }

        /// <summary>
        /// Realiza un test de conectividad (Ping) con los servidores de Binance.
        /// </summary>
        [HttpGet("ping")]
        public async Task<IActionResult> TestPing()
        {
            try
            {
                // Llamamos al endpoint de spot del sistema para verificar el ping
                var result = await _binanceRestClient.SpotApi.ExchangeData.PingAsync();

                if (result.Success)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Conexión exitosa con Binance API (Ping OK).",
                        Timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "No se pudo conectar con Binance.",
                        Error = result.Error?.Message
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Ocurrió un error interno al intentar conectar.",
                    Details = ex.Message
                });
            }
        }
    }
}
