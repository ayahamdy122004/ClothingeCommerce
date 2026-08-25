using E_Commerce.Entities.DTO.Models.Variation;

namespace E_Commerce.services.VariationProductServices
{
    public interface IVariationProductService
    {
        Task<VariationProductResponse> Create(
            int productId,
            CreateVariationProduct variationProduct);

        Task<VariationProductResponse> Update(
            int id,
            UpdateVariationProduct variationProduct);

        Task<IEnumerable<VariationProductResponse>> GetAll();

        Task<VariationProductResponse> GetById(int id);

        Task<bool> IsSkuExistAsync(
            string sku,
            int? excludeId = null);
    }
}