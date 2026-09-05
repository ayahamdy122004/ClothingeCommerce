using AutoMapper;
using E_Commerce.Entities;
using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.BRANDS;
using E_Commerce.Entities.Model;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;

namespace E_Commerce.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository repo;
        private readonly IMapper mapper;

        public BrandService(IBrandRepository repo)
        {
           this.repo = repo;
        }

        
        public async Task<IEnumerable<BrandResponseDTO>> GetAllAsync()
        {
            var brands = await repo.GetAllAsync();

            return brands.Select(b => new BrandResponseDTO
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                LogoUrl = b.LogoUrl,
                IsActive = b.IsActive
            });
        }

        public async Task<BrandResponseDTO?> CreateAsync(CreateBrandRequestDTO request)
        {
            if (await repo.IsNameExistAsync(request.Name))
                throw new Exception("Brand name already exists."); 
            var brand = new Brand
            {
                Name = request.Name,
                Description = request.Description,
                LogoUrl = request.LogoUrl,
                IsActive = true 
            };
            repo.Add(brand);
            await repo.SaveChangesAsync();
            return new BrandResponseDTO
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                LogoUrl = brand.LogoUrl,
                IsActive = brand.IsActive
            };
        }
        public async Task<BrandResponseDTO?> UpdateAsync(int id, UpdateBrandRequestDTO request)
        {
            var brand = await repo.GetByIdAsync(id);
            if (brand == null) return null;
            if (await repo.IsNameExistAsync(request.Name, id))
                throw new Exception("Brand name already exists.");
            brand.Name = request.Name;
            brand.Description = request.Description;
            brand.LogoUrl = request.LogoUrl;

            repo.Update(brand);
            await repo.SaveChangesAsync();

            return new BrandResponseDTO
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                LogoUrl = brand.LogoUrl,
                IsActive = brand.IsActive
            };
        }
        public async Task<bool> UpdateStatusAsync(int id, bool isActive)
        {
            var brand = await repo.GetByIdAsync(id);
            if (brand == null) return false;

            brand.IsActive = isActive;
            repo.Update(brand);
            await repo.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<BrandResponseDTO>> GetAllActiveBrandsAsync()
        {
            var brands = await repo.GetAllAsync();
            var activeBrands = brands.Where(b => b.IsActive);
       return  mapper.Map<IEnumerable<BrandResponseDTO>>(activeBrands);
        }
    }
}