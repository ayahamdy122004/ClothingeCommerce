namespace E_Commerce.Entities.DTO.Models.PRODUCTS
{
    public class ProductListResponseDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal? OriginalPrice { get; set; }
        public List<string> AvailableColors { get; set; } = new();
        public List<string> AvailableSizes { get; set; } = new();
        public string InStockStatus { get; set; } = string.Empty; // "In Stock" or "Out of Stock"
    }
}