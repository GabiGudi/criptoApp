using CriptoAppAPI.DTOs;

namespace CriptoAppAPI.Interfaces
{
    public interface ITransaccionServicio
    {
        Task<List<TransaccionDTO>> ObtenerTodas();
        Task<TransaccionDTO?> ObtenerPorId(int id);
        Task<TransaccionDTO> Crear(CrearTransaccionDTO dto);
        Task<TransaccionDTO?> Actualizar(int id, ActualizarTransaccionDTO dto);
        Task<bool> Eliminar(int id);
    }
}
