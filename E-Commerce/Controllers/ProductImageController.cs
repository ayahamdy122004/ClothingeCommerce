using E_Commerce.Entities.DTO.Models.ProductImages;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Helpers;
using E_Commerce.services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        // POST: api/productimage/upload
        [HttpPost("upload")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductImageResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadImages([FromForm] UploadImageRequestDTO request)
        {
            var result = await _productImageService.UploadImagesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/productimage/product/5
        [HttpGet("product/{productId:int}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductImageResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var result = await _productImageService.GetImagesByProductIdAsync(productId);
            return StatusCode(result.StatusCode, result);
        }

        // DELETE: api/productimage/5
        [HttpDelete("{imageId:int}")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var result = await _productImageService.DeleteImageAsync(imageId);
            return StatusCode(result.StatusCode, result);
        }
    }
}