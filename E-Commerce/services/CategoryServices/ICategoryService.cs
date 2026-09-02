using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.CATEGORIES;
using E_Commerce.Entities.Model;

namespace E_Commerce.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDTO>> GetAllAsync();
        Task<IEnumerable<Category>>AllCategoryIsActive(bool IsActive);
        Task<CategoryResponseDTO?> CreateAsync(CreateCategoryRequestDTO request);
        Task<CategoryResponseDTO?> UpdateAsync(int id, UpdateCategoryRequestDTO request);
        Task<bool> UpdateStatusAsync(int id, bool isActive);
    }
}