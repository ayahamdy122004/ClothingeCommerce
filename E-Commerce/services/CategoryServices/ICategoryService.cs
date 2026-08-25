using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.CATEGORIES;

namespace E_Commerce.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync();
        Task<CategoryResponse?> CreateAsync(CreateCategoryRequest request);
        Task<CategoryResponse?> UpdateAsync(int id, UpdateCategoryRequest request);
        Task<bool> UpdateStatusAsync(int id, bool isActive);
    }
}