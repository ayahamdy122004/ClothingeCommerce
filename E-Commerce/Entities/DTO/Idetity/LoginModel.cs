using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTO.Idetity
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
