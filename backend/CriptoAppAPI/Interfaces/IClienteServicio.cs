using CriptoAppAPI.DTOs;

namespace CriptoAppAPI.Interfaces
{
    public interface IClienteServicio
    {
        Task<List<ClienteDTO>> ObtenerTodos();
        Task<ClienteDTO> Crear(CrearClienteDTO dto); 
    }
}
