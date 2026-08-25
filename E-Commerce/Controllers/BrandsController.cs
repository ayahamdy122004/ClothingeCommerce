using E_Commerce.Entities;
using E_Commerce.Entities.DTO; // <-- ده السطر اللي بيحل إيرور الـ UpdateStatusRequest
using E_Commerce.Entities.DTO.Models.BRANDS;
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

        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpPost]
        [Authorize(Role.Administrator)]
        public async Task<IActionResult> Create([FromBody] CreateBrandRequestDTO request)
        {
            var brand = await _brandService.CreateAsync(request);
            return Ok(brand);
        }

        [HttpPut("{id}")]
      
        [Authorize(Role.Administrator)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBrandRequestDTO request)
        {
            var brand = await _brandService.UpdateAsync(id, request);
            if (brand == null)
                return NotFound(new { message = "Brand not found." });

            return Ok(brand);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Role.Administrator)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var result = await _brandService.UpdateStatusAsync(id, request.IsActive);
            if (!result)
                return NotFound(new { message = "Brand not found." });

            return Ok(new { message = "Brand status updated successfully." });
        }
    }
}