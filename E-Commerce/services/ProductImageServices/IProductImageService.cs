using E_Commerce.Entities.DTO.Models.ProductImages;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.services.ProductServices
{
    public interface IProductImageService
    {
        Task<ApiResponse<IEnumerable<ProductImageResponseDTO>>> UploadImagesAsync(UploadImageRequestDTO request);
        Task<ApiResponse<IEnumerable<ProductImageResponseDTO>>> GetImagesByProductIdAsync(int productId);
        Task<ApiResponse<bool>> DeleteImageAsync(int imageId);
    }
}