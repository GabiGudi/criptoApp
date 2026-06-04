using CriptoAppAPI.DTOs;
using CriptoAppAPI.Interfaces;
using CriptoAppAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CriptoAppAPI.Controllers
{
    [Route("transacciones")]
    [ApiController]
    public class TransaccionController : ControllerBase
    {
        private readonly ITransaccionServicio _transaccionServicio;

        public TransaccionController(ITransaccionServicio transaccionServicio)
        {
            _transaccionServicio = transaccionServicio;
        }

        // GET /transacciones
        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var transacciones = await _transaccionServicio.ObtenerTodas();
            return Ok(transacciones);
        }

        // POST /transacciones
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearTransaccionDTO dto)
        {
            // Validar que la cantidad sea mayor a 0
            if (dto.crypto_amount <= 0)
                return BadRequest("La cantidad de criptomoneda debe ser mayor a 0.");

            // Validar que la acción sea compra o venta
            if (dto.action != "purchase" && dto.action != "sale")
                return BadRequest("La acción debe ser 'purchase' o 'sale'.");

            var transaccion = await _transaccionServicio.Crear(dto);
            return Ok(transaccion);
        }
    }
}
