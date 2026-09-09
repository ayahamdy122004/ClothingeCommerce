using E_Commerce.Entities;
using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Models.BRANDS;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Helpers;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminBrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public AdminBrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BrandResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _brandService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BrandResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Role.Administrator)] 
        public async Task<IActionResult> Create([FromBody] CreateBrandRequestDTO request)
        {
            var result = await _brandService.CreateAsync(request);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BrandResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Role.Administrator)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBrandRequestDTO request)
        {
            var result = await _brandService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Role.Administrator)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var result = await _brandService.UpdateStatusAsync(id, request.IsActive);
            return StatusCode(result.StatusCode, result);
        }
    }
}