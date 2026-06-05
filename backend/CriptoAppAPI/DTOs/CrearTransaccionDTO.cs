namespace CriptoAppAPI.DTOs
{
    public class CrearTransaccionDTO
    {
        public string crypto_code { get; set; } 
        public string action { get; set; } 
        public decimal crypto_amount { get; set; } 
        public DateTime datetime { get; set; } 
    }
}
