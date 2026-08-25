using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTO.Models.BRANDS
{
    public class UpdateBrandRequestDTO
    {
        [Required(ErrorMessage = "Brand name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? LogoUrl { get; set; }
    }
}
