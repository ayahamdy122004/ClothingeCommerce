using E_Commerce.Entities.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Entities
{
    public class Product
    {
        public int Id { get; set; }

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
        [ForeignKey(nameof(BrandId))]
        public Brand? Brand { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        // الأسعار بـ decimal(18,2) زي ما طلب الـ PDF
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountPrice { get; set; }

        [MaxLength(500)]
        public string? CoverImageUrl { get; set; }

        [MaxLength(100)]
        public string? Material { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; } // Target Audience

        public string? CareInstructions { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductVariation> Variations { get; set; } = new List<ProductVariation>();
    }
}