using E_Commerce.Entities;
using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.CATEGORIES;
using E_Commerce.Entities.Model;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;

namespace E_Commerce.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryResponseDTO>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(c => new CategoryResponseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive
            });
        }

        public async Task<CategoryResponseDTO?> CreateAsync(CreateCategoryRequestDTO request)
        {
            if (await _categoryRepository.IsNameExistAsync(request.Name))
                throw new Exception("Category name already exists.");

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                IsActive = true
            };

            _categoryRepository.Add(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IsActive = category.IsActive
            };
        }

        public async Task<CategoryResponseDTO?> UpdateAsync(int id, UpdateCategoryRequestDTO request)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            if (await _categoryRepository.IsNameExistAsync(request.Name, id))
                throw new Exception("Category name already exists.");

            category.Name = request.Name;
            category.Description = request.Description;
            category.ImageUrl = request.ImageUrl;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IsActive = category.IsActive
            };
        }

        public async Task<bool> UpdateStatusAsync(int id, bool isActive)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            category.IsActive = isActive;
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return true;
        }
    }
}