using E_Commerce.Entities;
using E_Commerce.Entities.Model;

namespace E_Commerce.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<bool> IsNameExistAsync(string name, int? excludeId = null);
        void Add(Category category);
        void Update(Category category);
        Task SaveChangesAsync();
    }
}