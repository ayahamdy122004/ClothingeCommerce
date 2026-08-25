using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTO
{
 public class CreateBrandRequestDTO
    {
        [Required(ErrorMessage = "Brand name is required")]
        [MaxLength(100, ErrorMessage = "Brand name cannot exceed 100 characters")]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? LogoUrl { get; set; }
    }
}