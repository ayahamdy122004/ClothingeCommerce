using ClothingStore.Entities;

namespace E_Commerce.Repositorys.ProductRepo
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task<bool> IsSlugExistAsync(string slug, int? excludeId = null);
        Task SaveChangesAsync();
      //  Task<Product?> GetBySlugAsync(string slug);
    }
}
