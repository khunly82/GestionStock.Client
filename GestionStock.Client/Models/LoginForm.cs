using System.ComponentModel.DataAnnotations;

namespace GestionStock.Client.Models
{
    public class LoginForm
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
