using CriptoAppAPI.Interfaces;
using System.Text.Json;

namespace CriptoAppAPI.Services
{
    public class CriptoYaServicio : ICriptoYaServicio
    {
        private readonly HttpClient _httpClient;

        private readonly Dictionary<string, string> _codigos = new()
        {
            { "bitcoin", "btc" },
            { "eth", "eth" },
            { "usdc", "usdc" },
            { "sol", "sol" },
            { "dai", "dai" }
        };

        public CriptoYaServicio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ObtenerPrecio(string cryptoCode)
        {
            if (!_codigos.ContainsKey(cryptoCode.ToLower()))
                throw new Exception($"Criptomoneda '{cryptoCode}' no soportada.");

            string codigo = _codigos[cryptoCode.ToLower()];
            string url = $"https://criptoya.com/api/satoshitango/{codigo}/ars/1";

            string respuesta = await _httpClient.GetStringAsync(url);
            var datos = JsonSerializer.Deserialize<CriptoYaRespuesta>(respuesta);

            return datos.totalAsk; 
        }
    }

    public class CriptoYaRespuesta
    {
        public decimal ask { get; set; } 
        public decimal totalAsk { get; set; } 
        public decimal bid { get; set; } 
        public decimal totalBid { get; set; } 
    }
}
