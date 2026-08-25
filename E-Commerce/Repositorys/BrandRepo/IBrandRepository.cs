using E_Commerce.Entities;
using E_Commerce.Entities.Model;

namespace E_Commerce.Repositories.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(int id);
        Task<bool> IsNameExistAsync(string name, int? excludeId = null); // للتحقق من التكرار
        void Add(Brand brand);
        void Update(Brand brand);
        Task SaveChangesAsync();
    }
}