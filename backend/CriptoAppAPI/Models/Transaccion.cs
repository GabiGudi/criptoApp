using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriptoAppAPI.Models
{
    public class Transaccion
    {
        [Key] public int id { get; set; }

        [Required] public string crypto_code { get; set; }   

        [Required] public string action { get; set; }         

        [Column(TypeName = "decimal(18,8)")]
        [Required] public decimal crypto_amount { get; set; }  

        [Column(TypeName = "decimal(18,2)")] 
        [Required] public decimal money { get; set; }         

        [Required] public DateTime datetime { get; set; }

        public int? cliente_id { get; set; }      
    }
}
