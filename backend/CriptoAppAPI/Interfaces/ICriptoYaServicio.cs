namespace CriptoAppAPI.Interfaces
{
    public interface ICriptoYaServicio
    {
        Task<decimal> ObtenerPrecio(string cryptoCode);
    }
}
