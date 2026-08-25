using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.BRANDS;

namespace E_Commerce.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponseDTO>> GetAllAsync();
        Task<BrandResponseDTO?> CreateAsync(CreateBrandRequestDTO request);
        Task<BrandResponseDTO?> UpdateAsync(int id, UpdateBrandRequestDTO request);
        Task<bool> UpdateStatusAsync(int id, bool isActive);
    }
}