using ArbNet.Models;
using ArbNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArbNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BinanceP2PController : ControllerBase
    {
        private readonly BinanceP2PService _p2PService;
        private readonly IConfiguration _configuration;

        public BinanceP2PController(BinanceP2PService p2PService, IConfiguration configuration)
        {
            _p2PService = p2PService;
            _configuration = configuration;
        }

        /// <summary>
        /// Obtiene el historial de órdenes P2P para el panel de arbitraje.
        /// </summary>
        [HttpGet("historial-p2p")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<BinanceP2POrder>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCompletedP2pOrders(
            [FromHeader] string? apiKey = null,
            [FromHeader] string? secretKey = null)
        {
            // Intentamos leer la configuración; si no existe o falla, por defecto usamos la simulación (true)
            if (!bool.TryParse(_configuration["BinanceConfig:UseTestnet"], out bool useTestnet))
            {
                useTestnet = true;
            }

            // Validación de credenciales solo si se apaga el modo simulación/testnet
            if (!useTestnet)
            {
                string finalApiKey = !string.IsNullOrEmpty(apiKey)
                    ? apiKey
                    : _configuration["BinanceConfig:ApiKey"] ?? string.Empty;

                string finalSecretKey = !string.IsNullOrEmpty(secretKey)
                    ? secretKey
                    : _configuration["BinanceConfig:SecretKey"] ?? string.Empty;

                if (string.IsNullOrEmpty(finalApiKey) || string.IsNullOrEmpty(finalSecretKey))
                {
                    return BadRequest("Error: Las credenciales de producción son obligatorias si UseTestnet es false.");
                }
            }

            // Trae las órdenes (si useTestnet es true, traerá la lista de 10 órdenes simuladas y combinadas)
            var historial = await _p2PService.GetOrderHistoryAsync(useTestnet);
            return Ok(historial);
        }

        //====================
        //add Eliezer
        //====================
        [HttpGet("summary")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArbitrageSummary))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ArbitrageSummary>> GetSummary()
        {
            try
            {
                // Lee la configuración para saber si está en modo testnet
                if (!bool.TryParse(_configuration["BinanceConfig:UseTestnet"], out bool useTestnet))
                {
                    useTestnet = true;
                }

                // Pasa el parámetro useTestnet para que use los mismos datos que el historial
                var summary = await _p2PService.GetArbitrageCalculationsAsync(useTestnet);
                return Ok(summary);
            }
            catch (System.Exception ex)
            {
                // Esto le avisa a Swagger y a React si algo sale mal en los cálculos
                return StatusCode(500, $"Error interno al calcular arbitraje: {ex.Message}");
            }
        }
    }
}