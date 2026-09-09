using E_Commerce.Entities;
using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.CATEGORIES;
using E_Commerce.Entities.DTO.ResponseAPIs;
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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("active")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllActiveCategories()
        {
            var result = await _categoryService.GetAllActiveCategoriesAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<CategoryResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequestDTO request)
        {
            var result = await _categoryService.CreateAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<CategoryResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryRequestDTO request)
        {
            var result = await _categoryService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var result = await _categoryService.UpdateStatusAsync(id, request.IsActive);
            return StatusCode(result.StatusCode, result);
        }
    }
}