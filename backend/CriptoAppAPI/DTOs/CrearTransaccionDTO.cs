namespace CriptoAppAPI.DTOs
{
    public class CrearTransaccionDTO
    {
        // son los datos que llegan desde el frontend para crear una transaccion

        public string crypto_code { get; set; } // bitcoin, usdc, eth
        public string action { get; set; } // compra o venta
        public decimal crypto_amount { get; set; } // cantidad de criptomoneda a comprar o vender
        public DateTime datetime { get; set; } // fecha y hora de la transaccion
    }
}
