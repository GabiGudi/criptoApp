using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriptoAppAPI.Models
{
    public class Transaccion
    {
        [Key] public int id { get; set; }

        [Required] public string crypto_code { get; set; }    // "bitcoin", "usdc", "eth"

        [Required] public string action { get; set; }          // "purchase" o "sale"

        [Column(TypeName = "decimal(18,8)")] // Lo agregamos ya que  los campos decimal necesitan precisión definida - 8 decimales para cripto
        [Required] public decimal crypto_amount { get; set; }  // cantidad de cripto comprada/vendida

        [Column(TypeName = "decimal(18,2)")] // 2 decimales para pesos
        [Required] public decimal money { get; set; }          // total en pesos ARS

        [Required] public DateTime datetime { get; set; }

        [Required] public int cliente_id { get; set; }          // FK al cliente

        public Cliente? cliente { get; set; }       // relación de navegación
    }
}
