using E_Commerce.Entities.DTO.Models.PRODUCTIMAGEFolder;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.services.ProductServices
{
    public interface IProductImageService
    {
        Task<ApiResponse<ProductImageResponseDTO>> UploadImageAsync(UploadImageDTO dto);
        Task<ApiResponse<bool>> ChangeDisplayOrderAsync(ChangeOrderDTo dto);
        Task<ApiResponse<bool>> SelectCoverImageAsync(SelectCoverDTO dto);
        Task<ApiResponse<bool>> DeleteImageAsync(int imgId);
        Task<ApiResponse<IEnumerable<ProductImageResponseDTO>>> GetImagesByProductIdAsync(int productId);
        Task<ApiResponse<ProductImageResponseDTO>> UpdateImage(int ImgId);
    }
}