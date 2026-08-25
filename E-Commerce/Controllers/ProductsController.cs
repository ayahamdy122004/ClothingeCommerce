using E_Commerce.Entities.DTO.Models.PRODUCTS;
using E_Commerce.services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//طبعا ي استاذ منتور انا عامله كومنت علي  [Authorize(Role.Administrator)] عشان اعرف اضيف و اعرفه انك عايزاه من غير كومنت 
namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService service;
        public ProductsController(IProductService service)
        {
            this.service = service;

        }
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAll()
        {
            var products = await service.GetAll();
            return Ok(products);
        }

        [HttpPut("Update-Product")]
       // [Authorize(Role.Administrator)]
        public async Task<IActionResult> Update(int id, [FromForm] UPdateProductRequestDTO up)
        {
            var pro = await service.UpdateProduct(id, up);
            if (pro != null)
            {
                return Ok(pro);

            }
            return NotFound("Product is not found");
        }
        //[Authorize(Roles = "Administrator")]
        [HttpPost("Add-Product")]
        public async Task<IActionResult> Add([FromForm] CreateProductRequestDTO pro)
        {
            var p=await service.AddProduct(pro);    
            if (p != null) 
                return Ok(p);
            return BadRequest("this item id null");
        }
        [HttpPatch("Update-Status/{id}")]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool isActive)
        {
            var result = await service.UpdateStatusAsync(id, isActive);
            if (result)
            {
                return Ok(new { message = "Product status updated successfully." });
            }
            else
            {
                return NotFound(new { message = "Product not found." });
            }
        }   
    }
}