using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTO.Models.CATEGORIES
{
    public class CreateCategoryRequestDTO
    {
        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }
    }

}
