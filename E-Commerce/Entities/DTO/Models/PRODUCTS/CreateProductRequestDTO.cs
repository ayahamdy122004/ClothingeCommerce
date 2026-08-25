using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Entities.DTO.Models.PRODUCTS
{
    public class CreateProductRequestDTO 
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(220)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ShortDescription { get; set; }

        public string? FullDescription { get; set; }

        // لازمة أرقام عشان الـ Foreign Key
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public decimal BasePrice { get; set; }

        [MaxLength(500)]
        public string? CoverImageUrl { get; set; }

        [MaxLength(100)]
        public string? Material { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }

        public string? CareInstructions { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
    }
}