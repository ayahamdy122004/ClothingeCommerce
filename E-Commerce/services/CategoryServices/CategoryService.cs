using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.CATEGORIES;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;

namespace E_Commerce.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)=>  _categoryRepository = categoryRepository;
        public async Task<ApiResponse<IEnumerable<CategoryResponseDTO>>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var dtos = categories.Select(c => new CategoryResponseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive
            });

            return new ApiResponse<IEnumerable<CategoryResponseDTO>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Categories retrieved successfully.",
                Data = dtos
            };
        }

        public async Task<ApiResponse<CategoryResponseDTO>> CreateAsync(CreateCategoryRequestDTO request)
        {
            if (await _categoryRepository.IsNameExistAsync(request.Name))
            {
                return new ApiResponse<CategoryResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Category name already exists.",
                    Errors = new { Name = "Category name already exists." }
                };
            }

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                IsActive = true
            };

            _categoryRepository.Add(category);
            await _categoryRepository.SaveChangesAsync();

            return new ApiResponse<CategoryResponseDTO>
            {
                StatusCode = 201,
                Success = true,
                Message = "Category created successfully.",
                Data = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    IsActive = category.IsActive
                }
            };
        }
        public async Task<ApiResponse<CategoryResponseDTO>> UpdateAsync(int id, UpdateCategoryRequestDTO request)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ApiResponse<CategoryResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Category not found."
                };
            }

            if (await _categoryRepository.IsNameExistAsync(request.Name, id))
            {
                return new ApiResponse<CategoryResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Category name already exists.",
                    Errors = new { Name = "Category name already exists." }
                };
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.ImageUrl = request.ImageUrl;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return new ApiResponse<CategoryResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "Category updated successfully.",
                Data = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    IsActive = category.IsActive
                }
            };
        }
        public async Task<ApiResponse<bool>> UpdateStatusAsync(int id, bool isActive)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ApiResponse<bool>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Category not found.",
                    Data = false
                };
            }

            category.IsActive = isActive;
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                StatusCode = 200,
                Success = true,
                Message = "Category status updated successfully.",
                Data = true
            };
        }
        public async Task<ApiResponse<IEnumerable<CategoryResponseDTO>>> GetAllActiveCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var dtos = categories.Where(c => c.IsActive).Select(c => new CategoryResponseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive
            });

            return new ApiResponse<IEnumerable<CategoryResponseDTO>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Active categories retrieved successfully.",
                Data = dtos
            };
        }
    }
}