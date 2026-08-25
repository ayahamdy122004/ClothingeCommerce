using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.BRANDS;

namespace E_Commerce.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponse>> GetAllAsync();
        Task<BrandResponse?> CreateAsync(CreateBrandRequest request);
        Task<BrandResponse?> UpdateAsync(int id, UpdateBrandRequest request);
        Task<bool> UpdateStatusAsync(int id, bool isActive);
    }
}