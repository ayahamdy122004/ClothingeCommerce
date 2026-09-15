using AutoMapper;
using E_Commerce.Entities;
using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.BRANDS;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;
using System.Net.Http.Headers;

namespace E_Commerce.Services
{
    public class BrandService : IBrandService
    {
        #region
        private readonly IBrandRepository repo;
        private readonly IMapper mapper;

        public BrandService(IBrandRepository repo, IMapper mapper)
        {
           this.repo = repo;
            this.mapper = mapper;
        }
        #endregion

        public async Task<ApiResponse<IEnumerable<BrandResponseDTO>>> GetAllAsync()
        {
            var brands = await repo.GetAllAsync();

         
            return new ApiResponse<IEnumerable<BrandResponseDTO>>
            {
                Data = brands.Select(b => new BrandResponseDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    LogoUrl = b.LogoUrl,
                    IsActive = b.IsActive
                }),
                StatusCode = 200,
                Message = "Brands retrieved successfully.",
                Success = true
            };

        }
        public async Task<ApiResponse<BrandResponseDTO>?> CreateAsync(CreateBrandRequestDTO request)
        {
            if (await repo.IsNameExistAsync(request.Name))
                return new ApiResponse<BrandResponseDTO>
                { StatusCode =400,
                   // Data = null,
                    Message = "Brand name already exists.",
                  //  Success = false
                  Errors = new { Name = "Brand name already exists." }
                };
            var brand = new Brand
            {
                Name = request.Name,
                Description = request.Description,
                LogoUrl = request.LogoUrl,
                IsActive = true 
            };
            repo.Add(brand);
            await repo.SaveChangesAsync();
            return new ApiResponse<BrandResponseDTO>
            {
                Data = new BrandResponseDTO
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Description = brand.Description,
                    LogoUrl = brand.LogoUrl,
                    IsActive = brand.IsActive
                },
                StatusCode = 201,
                Message = "Brand created successfully.",
                Success = true
            };
        }
        public async Task<ApiResponse<BrandResponseDTO>?> UpdateAsync(int id, UpdateBrandRequestDTO request)
        {
            var brand = await repo.GetByIdAsync(id);
            if (brand == null) 
                return new ApiResponse<BrandResponseDTO>
                {
                    StatusCode = 404,
                    Message = "Brand not found.",
                    Success = false
                };
            if (await repo.IsNameExistAsync(request.Name, id))
                return new ApiResponse<BrandResponseDTO>
                {
                    StatusCode = 400,
                    Message = "Brand name already exists.",
                    
                    Success = false
                };


            brand.Name = request.Name;
            brand.Description = request.Description;
            brand.LogoUrl = request.LogoUrl;

            repo.Update(brand);
            await repo.SaveChangesAsync();

          
            return new ApiResponse<BrandResponseDTO> {
                Data = new BrandResponseDTO
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Description = brand.Description,
                    LogoUrl = brand.LogoUrl,
                    IsActive = brand.IsActive
                },
                StatusCode = 200,
                Message = "Brand updated successfully.",
                Success = true
            };
        }
        public async Task<ApiResponse<bool>> UpdateStatusAsync(int id, bool isActive)
        {
            var brand = await repo.GetByIdAsync(id);
            if (brand == null)
                return new ApiResponse<bool>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "brand not found"


                };

            brand.IsActive = isActive;
            repo.Update(brand);
            await repo.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Brand has been updated successfully",
                StatusCode=200,
                Data = true

            };
        }

        public async Task<ApiResponse<IEnumerable<BrandResponseDTO>>> GetAllActiveBrandsAsync()
        {
            var brands = await repo.GetAllAsync();
            var activeBrands = brands.Where(b => b.IsActive);
      ;
            return new ApiResponse<IEnumerable<BrandResponseDTO>>
           {
               Data = mapper.Map<IEnumerable<BrandResponseDTO>>(activeBrands),
               Message="All brands are active",
               StatusCode=200
           };
        }
    }
}