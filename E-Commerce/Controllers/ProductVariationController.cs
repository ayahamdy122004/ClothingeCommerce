using E_Commerce.Entities.DTO.Models.Variation;
using E_Commerce.services.VariationProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariationProductController : ControllerBase
    {
        private readonly IVariationProductService variationProductService;

        public VariationProductController(
            IVariationProductService variationProductService)
        {
            this.variationProductService = variationProductService;
        }

       // [Authorize(Role.Administrator)]
        [HttpPost("Create{productId}")]
        public async Task<IActionResult> Create(
            int productId,
            CreateVariationProductDTO variationProduct)
        {
            try
            {
                var result = await variationProductService
                    .Create(productId, variationProduct);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      //  [Authorize(Role.Administrator)]
        [HttpPut("Update({id})")]
        public async Task<IActionResult> Update(
            int id,
            UpdateVariationProductDTO variationProduct)
        {
            try
            {
                var result = await variationProductService
                    .Update(id, variationProduct);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await variationProductService.GetAll();

            return Ok(result);
        }

        [HttpGet("GetById{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await variationProductService.GetById(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("check-sku")]
        public async Task<IActionResult> CheckSku(
            string sku,
            int? excludeId = null)
        {
            var result = await variationProductService
                .IsSkuExistAsync(sku, excludeId);

            return Ok(result);
        }
    }
}