using E_Commerce.Entities.DTO.Models.Variation;

namespace E_Commerce.services.VariationProductServices
{
    public interface IVariationProductService
    {
        Task<VariationProductResponseDTO> Create(
            int productId,
            CreateVariationProductDTO variationProduct);

        Task<VariationProductResponseDTO> Update(
            int id,
            UpdateVariationProductDTO variationProduct);

        Task<IEnumerable<VariationProductResponseDTO>> GetAll();

        Task<VariationProductResponseDTO> GetById(int id);

        Task<bool> IsSkuExistAsync(
            string sku,
            int? excludeId = null);
    }
}