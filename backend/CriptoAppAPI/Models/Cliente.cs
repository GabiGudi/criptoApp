using System.ComponentModel.DataAnnotations;

namespace CriptoAppAPI.Models
{
    public class Cliente
    {
        [Key] public int id { get; set; }

        [Required] public string nombre { get; set; }

        [Required] public string email { get; set; }
    }
}
