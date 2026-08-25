using E_Commerce.Entities.Model;

namespace E_Commerce.Repositorys.VariationRepo
{
    public interface IVariationRepository
    {
        Task<ProductVariation> GetById(int id);
        Task<IEnumerable<ProductVariation>>GetAll();
        Task<ProductVariation> Add(ProductVariation variation);
        Task<ProductVariation> Update(ProductVariation variation);
        Task<bool> IsSkuExistAsync(string sku, int? excludeId = null);
    }
}
