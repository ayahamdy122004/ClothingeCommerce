using E_Commerce.Entities.DTO.Models.ProductImages;
using E_Commerce.Helpers;
using E_Commerce.services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            this.productImageService = productImageService;
        }

         [Authorize(Role.Administrator)]
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImages([FromForm] UploadImageRequestDTO request)
        {
            try
            {
                var result = await productImageService.UploadImagesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetImagesByProductId{productId}")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            try
            {
                var result = await productImageService.GetImagesByProductIdAsync(productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Role.Administrator)]
        [HttpDelete("Delete{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            try
            {
                var result = await productImageService.DeleteImageAsync(imageId);

                if (!result)
                {
                    return NotFound("Image not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}