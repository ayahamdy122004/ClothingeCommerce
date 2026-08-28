using E_Commerce.Entities.DTO.Models.ProductImages;
using E_Commerce.Entities.Model;

namespace E_Commerce.services.ProductServices
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImageUploadItemDTO>> UploadImagesAsync(UploadImageRequestDTO request);
        Task<bool> DeleteImageAsync(int imageId);
        Task<IEnumerable<ProductImageUploadItemDTO>> GetImagesByProductIdAsync(int productId);
    }
}