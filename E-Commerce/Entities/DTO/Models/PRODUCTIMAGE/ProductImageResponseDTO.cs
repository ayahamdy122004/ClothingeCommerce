namespace E_Commerce.Entities.DTO.Models.ProductImages
{
    public class ProductImageResponseDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? AlternativeText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsCover { get; set; }
    }
}