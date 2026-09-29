using E_Commerce.Entities.DTO.Models.PRODUCTIMAGEFolder;
using E_Commerce.Entities.DTO.Models.PRODUCTS;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Helpers;
using E_Commerce.services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

      
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDTO dto)
        {
            var result = await _productImageService.UploadImageAsync(dto);
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        // 2. Select Cover Image
        [HttpPut("select-cover")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SelectCoverImage([FromBody] SelectCoverDTO dto)
        {
            var result = await _productImageService.SelectCoverImageAsync(dto);
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        // 3. Change Image Display Order
        [HttpPut("change-order")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeDisplayOrder([FromBody] ChangeOrderDTo dto)
        {
            var result = await _productImageService.ChangeDisplayOrderAsync(dto);
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        // 4. Get Images by Product Id
        [HttpGet("product/{productId}")]
       
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var result = await _productImageService.GetImagesByProductIdAsync(productId);
            if(result == null)
                {
                return NotFound(result);
            }
            return Ok(result);
        }

     
        [HttpDelete("{imgId}")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(int imgId)
        {
            var result = await _productImageService.DeleteImageAsync(imgId);
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("UpdateCover/{ImgId}")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateImage(int ImgId)
        {
            var result = await _productImageService.UpdateImage(ImgId);
            if(result==null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}