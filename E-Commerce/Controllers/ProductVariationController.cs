using E_Commerce.Entities.DTO.Models.Variation;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Helpers;
using E_Commerce.services.VariationProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariationProductController : ControllerBase
    {
        private readonly IVariationProductService _variationProductService;

        public VariationProductController(IVariationProductService variationProductService)
        {
            _variationProductService = variationProductService;
        }

        // POST: api/variationproduct/product/5
        [HttpPost("product/{productId:int}")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<VariationProductResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(int productId, [FromBody] CreateVariationProductDTO variationProduct)
        {
            var result = await _variationProductService.Create(productId, variationProduct);
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/variationproduct/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<VariationProductResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVariationProductDTO variationProduct)
        {
            var result = await _variationProductService.Update(id, variationProduct);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/variationproduct
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VariationProductResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _variationProductService.GetAll();
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/variationproduct/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VariationProductResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _variationProductService.GetById(id);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/variationproduct/check-sku?sku=ABC-123
        [HttpGet("check-sku")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckSku([FromQuery] string sku, [FromQuery] int? excludeId = null)
        {
            var result = await _variationProductService.IsSkuExistAsync(sku, excludeId);
            return StatusCode(result.StatusCode, result);
        }
    }
}