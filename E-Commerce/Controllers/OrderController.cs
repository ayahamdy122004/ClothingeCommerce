using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.services.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public OrderController(IOrderService orderService)
        {
        }
        public async Task<IActionResult> GetOrders(string userId)
        {
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddOrder([FromBody] CheckoutDTO order)
        {
        }
        public async Task<IActionResult> GetOrderById(int id)
        {
            }
                {
        }
        public async Task<IActionResult> CancelOrder(int orderId)
        {
        }
    }
}
