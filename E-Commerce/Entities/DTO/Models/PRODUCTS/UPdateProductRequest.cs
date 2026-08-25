using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTO.Models.PRODUCTS
{
    public class UpdateProductRequest
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

        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public decimal BasePrice { get; set; }

        public decimal? DiscountPrice { get; set; }

        [MaxLength(500)]
        public string? CoverImageUrl { get; set; }

        public string? Gender { get; set; }

        public string? CareInstructions { get; set; }

        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
    }
}