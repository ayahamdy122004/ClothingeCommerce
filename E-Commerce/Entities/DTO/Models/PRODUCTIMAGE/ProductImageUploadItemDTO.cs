namespace E_Commerce.Entities.DTO.Models.ProductImages
{
    public class ProductImageUploadItemDTO
    {
        public IFormFile File { get; set; } = null!;
        public string? AlternativeText { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsCover { get; set; } = false;
    }
}