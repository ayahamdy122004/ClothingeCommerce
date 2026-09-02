using E_Commerce.Entities.DTO.Models.CART;
using E_Commerce.services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        [HttpGet]
        public IActionResult GetCart()
        {
            var cart = cartService.GetCart();
            return Ok(cart);
        }
        [HttpPost]
        public IActionResult AddToCart([FromBody] AddCartDTO item)
        {
            cartService.AddToCartAsync(item);
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateCart([FromBody] UpdateCartDTO item)
        {
            cartService.UpdateQuantityAsync(item);
            return Ok();
        }   
        [HttpDelete]
        public IActionResult RemoveFromCart([FromQuery] int productId)
        {
            cartService.RemoveItemAsync(productId);
            return Ok();
        }
        [HttpDelete("clear")]
        public IActionResult ClearCart()
        {
            cartService.ClearCartAsync();
            return Ok();
        }
    }
}
