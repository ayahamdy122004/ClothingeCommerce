using E_Commerce.Entities.Model;

namespace E_Commerce.Repositorys.ProductImageRepo
{
    public interface IProductImageRepository
    {
        Task<ProductImage> UploadImageAsync(ProductImage product);
        Task<ProductImage?> GetByIdAsync(int imgId);
        Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId);
        Task ResetCoverImagesAsync(int productId);
        Task UpdateImageAsync(ProductImage image);
        Task DeleteImageAsync(ProductImage image);
    }
}