using E_Commerce.Entities.DTO.Models.Variation;

namespace E_Commerce.Entities.DTO.Models.PRODUCTS
{
    public class ProductDetailsResponseDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public string? CoverImage { get; set; }
        public List<string> AdditionalImages { get; set; } = new();
        public List<string> AvailableColors { get; set; } = new();
        public List<string> AvailableSizes { get; set; } = new();
        public string? Material { get; set; }
        public string? Gender { get; set; }
        public string? CareInstructions { get; set; }
        public string InStockStatus { get; set; } = string.Empty;
        public List<VariationProductResponseDTO> Variations { get; set; } = new();
    }
}