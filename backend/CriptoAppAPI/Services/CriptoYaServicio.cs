using CriptoAppAPI.Interfaces;
using System.Text.Json;

namespace CriptoAppAPI.Services
{
    public class CriptoYaServicio : ICriptoYaServicio
    {
        private readonly HttpClient _httpClient;

        // Mapeamos nuestros códigos al formato que usa CriptoYa en la URL
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

            return datos.totalAsk; // precio real que pagás al comprar
        }
    }

    // Clase para leer la respuesta JSON de CriptoYa
    public class CriptoYaRespuesta
    {
        public decimal ask { get; set; } // precio de venta
        public decimal totalAsk { get; set; } // precio con comisión incluida
        public decimal bid { get; set; } // precio de compra
        public decimal totalBid { get; set; } // precio con comisión incluida
    }
}
