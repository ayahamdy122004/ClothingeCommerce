using E_Commerce.Entities;
using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.CATEGORIES;
using E_Commerce.Helpers;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
   
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpPost("AddCategory")]
        [Authorize(Role.Administrator)]

        public async Task<IActionResult> Create([FromBody] CreateCategoryRequestDTO request)
        {
            var category = await _categoryService.CreateAsync(request);
            return Ok(category);
        }

        [HttpPut("Update{id}")]
        [Authorize(Role.Administrator)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryRequestDTO request)
        {
            var category = await _categoryService.UpdateAsync(id, request);
            if (category == null)
                return NotFound(new { message = "Category not found." });

            return Ok(category);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Role.Administrator)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var result = await _categoryService.UpdateStatusAsync(id, request.IsActive);
            if (!result)
                return NotFound(new { message = "Category not found." });

            return Ok(new { message = "Category status updated successfully." });
        }
    }
}