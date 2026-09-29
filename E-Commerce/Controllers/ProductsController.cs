using E_Commerce.Entities;
using E_Commerce.Entities.DTO.Models.Common;
using E_Commerce.Entities.DTO.Models.PRODUCTS;
using E_Commerce.Entities.DTO.Models.PRODUCTS.ProductFilterAndSearch;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service)=>_service = service;
      
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResponseDTO<ProductResponseDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryDTO query)
        {
            var result = await _service.GetProducts(query);
            if (result == null)
            {
                return NotFound(result);
            }
            else
            {
                return Ok(result);
            }
        }

        
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductResponseDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            if (result == null)
            {
                return NotFound(result);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("customer-list")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductListResponseDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProductListForCustomer()
        {
            var result = await _service.GetProductListForCustomerAsync();
            if (result == null)
            {
                return NotFound(result);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpGet("GetProductDetailsForCustomer/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProductDetailsResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProductDetailsForCustomer(int id)
        {
            var result = await _service.GetProductDetailsByIdAsync(id);
            if (result == null)
            {
                return NotFound(result);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpGet("slug/{slug}")]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProductBySlug(string slug)
        {
            var result = await _service.GetProductBySlug(slug);
            if (result == null)
            {
                return NotFound(result);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpPost("AddProduct")]
       [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Add([FromForm] CreateProductRequestDTO pro)
        {
            var result = await _service.AddProduct(pro);
            if (result == null)
            {
                return NotFound(result);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpPut("{id:int}")]
      //  [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> Update(int id, [FromForm] UPdateProductRequestDTO up)
        {
            var result = await _service.UpdateProduct(id, up);
            if (result == null)
            {
                return NotFound(result);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpPatch("{id:int}/status")]
     //   [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var result = await _service.UpdateStatusAsync(id, request.IsActive);
            if (result == null)
            {
                return NotFound(result);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}