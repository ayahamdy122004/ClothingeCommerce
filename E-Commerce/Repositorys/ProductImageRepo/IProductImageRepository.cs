using E_Commerce.Entities.Model;

namespace E_Commerce.Repositorys.ProductImageRepo
{
    public interface IProductImageRepository
    {
        Task AddRangeAsync(IEnumerable<ProductImage> images);
        Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId);
        Task ResetCoverImagesAsync(int productId);
        Task<ProductImage?> GetByIdAsync(int id);
        Task DeleteAsync(ProductImage image);
    }
}