using CriptoAppAPI.DTOs;

namespace CriptoAppAPI.Interfaces
{
    public interface ITransaccionServicio
    {
        Task<List<TransaccionDTO>> ObtenerTodas();
        Task<TransaccionDTO> Crear(CrearTransaccionDTO dto);
    }
}
