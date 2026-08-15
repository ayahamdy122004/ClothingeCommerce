using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTO.Idetity
{
    // 1. لتأكيد الإيميل
    public class ConfirmEmail
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}