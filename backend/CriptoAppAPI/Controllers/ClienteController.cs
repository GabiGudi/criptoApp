using CriptoAppAPI.DTOs;
using CriptoAppAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CriptoAppAPI.Controllers
{ 
    [ApiController]
    [Route("clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteServicio _clienteServicio;

        public ClienteController(IClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        // GET /clientes
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var clientes = await _clienteServicio.ObtenerTodos();
            return Ok(clientes);
        }

        //POST /clientes
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearClienteDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.nombre))
                return BadRequest("El nombre no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(dto.email) || !dto.email.Contains("@"))
                return BadRequest("El email no es válido");

            var cliente = await _clienteServicio.Crear(dto);
            return Ok(cliente);
        }
    }
}
