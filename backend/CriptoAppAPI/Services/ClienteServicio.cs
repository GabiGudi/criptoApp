using CriptoAppAPI.DTOs;
using CriptoAppAPI.Interfaces;
using CriptoAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CriptoAppAPI.Services
{
    public class ClienteServicio : IClienteServicio
    {
        private readonly AppDbContext _contexto;

        public ClienteServicio(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<ClienteDTO>> ObtenerTodos()
        {
            return await _contexto.Clientes
                .Select(c => new ClienteDTO
                {
                    id = c.id,
                    nombre = c.nombre,
                    email = c.email
                })
                .ToListAsync();
        }

        public async Task<ClienteDTO> Crear(CrearClienteDTO dto)
        {
            var cliente = new Cliente
            {
                nombre = dto.nombre,
                email = dto.email
            };

            _contexto.Clientes.Add(cliente);
            await _contexto.SaveChangesAsync();

            return new ClienteDTO
            {
                id = cliente.id,
                nombre = cliente.nombre,
                email = cliente.email
            };
        }
    }
}