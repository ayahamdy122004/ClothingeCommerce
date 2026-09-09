using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.CATEGORIES;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<IEnumerable<CategoryResponseDTO>>> GetAllAsync();
        Task<ApiResponse<IEnumerable<CategoryResponseDTO>>> GetAllActiveCategoriesAsync();
        Task<ApiResponse<CategoryResponseDTO>> CreateAsync(CreateCategoryRequestDTO request);
        Task<ApiResponse<CategoryResponseDTO>> UpdateAsync(int id, UpdateCategoryRequestDTO request);
        Task<ApiResponse<bool>> UpdateStatusAsync(int id, bool isActive);
    }
}