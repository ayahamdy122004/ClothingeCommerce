using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTO.Models.Variation
{
    public class CreateVariationProductDTO
    {
        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Size is required")]
        public string Size { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        public string SKU { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int StockQuantity { get; set; }

        public decimal? PriceAdjustment { get; set; }

        public bool IsActive { get; set; } = true;
    }
}