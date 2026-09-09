using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.BRANDS;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.Services.Interfaces
{
    public interface IBrandService
    {
        Task<ApiResponse<IEnumerable<BrandResponseDTO>>> GetAllAsync();
        Task<ApiResponse<IEnumerable<BrandResponseDTO>>> GetAllActiveBrandsAsync();
        Task<ApiResponse<BrandResponseDTO>?> CreateAsync(CreateBrandRequestDTO request);
        Task<ApiResponse<BrandResponseDTO>?> UpdateAsync(int id, UpdateBrandRequestDTO request);
        Task<ApiResponse<bool>> UpdateStatusAsync(int id, bool isActive);
    }
}