
using System.ComponentModel.DataAnnotations;


namespace DataLogic.Mappings.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
