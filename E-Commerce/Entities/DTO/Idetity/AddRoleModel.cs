using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTO.Idetity
{
    public class AddRoleModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
