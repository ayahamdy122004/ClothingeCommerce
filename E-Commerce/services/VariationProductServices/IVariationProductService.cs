using E_Commerce.Entities.DTO.Models.Variation;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.services.VariationProductServices
{
    public interface IVariationProductService
    {
        Task<ApiResponse<VariationProductResponseDTO>> Create(int productId, CreateVariationProductDTO variationProduct);
        Task<ApiResponse<VariationProductResponseDTO>> Update(int id, UpdateVariationProductDTO variationProduct);
        Task<ApiResponse<IEnumerable<VariationProductResponseDTO>>> GetAll();
        Task<ApiResponse<VariationProductResponseDTO>> GetById(int id);
        Task<ApiResponse<bool>> IsSkuExistAsync(string sku, int? excludeId = null);
    }
}