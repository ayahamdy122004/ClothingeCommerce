using Microsoft.AspNetCore.Http;

namespace E_Commerce.Entities.DTO.Models.ProductImages
{
    public class UploadImageRequestDTO
    {
        public int ProductId { get; set; }
        public List<ProductImageUploadItemDTO> Images { get; set; } = new();
    }
}