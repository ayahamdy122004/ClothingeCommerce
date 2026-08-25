using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.CATEGORIES;

namespace E_Commerce.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDTO>> GetAllAsync();
        Task<CategoryResponseDTO?> CreateAsync(CreateCategoryRequestDTO request);
        Task<CategoryResponseDTO?> UpdateAsync(int id, UpdateCategoryRequestDTO request);
        Task<bool> UpdateStatusAsync(int id, bool isActive);
    }
}