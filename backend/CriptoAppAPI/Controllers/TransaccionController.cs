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
            if (dto.crypto_amount <= 0)
                return BadRequest("La cantidad de criptomoneda debe ser mayor a 0.");

            if (dto.action != "purchase" && dto.action != "sale")
                return BadRequest("La acción debe ser 'purchase' o 'sale'.");

            var transaccion = await _transaccionServicio.Crear(dto);
            return Ok(transaccion);
        }

        // GET /transacciones/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var transaccion = await _transaccionServicio.ObtenerPorId(id);

            if (transaccion == null)
                return NotFound($"No existe una transacción con id {id}.");

            return Ok(transaccion);
        }

        // PATCH /transacciones/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarTransaccionDTO dto)
        {
            var transaccion = await _transaccionServicio.Actualizar(id, dto);

            if (transaccion == null)
                return NotFound($"No existe una transacción con id {id}.");

            return Ok(transaccion);
        }

        // DELETE /transacciones/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _transaccionServicio.Eliminar(id);

            if (!resultado)
                return NotFound($"No existe una transacción con id {id}.");

            return Ok("Transacción eliminada correctamente.");
        }
    }
}
