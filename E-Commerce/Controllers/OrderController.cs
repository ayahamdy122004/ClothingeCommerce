using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.DTO.ResponseAPIs;
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
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("user/{userId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrderResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrders(string userId)
        {
            var result = await _orderService.GetOrders(userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddOrder([FromBody] CheckoutDTO order)
        {
            var result = await _orderService.AddOrder(order);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{orderId:int}/status")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromQuery] string status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("{orderId:int}/cancel")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var result = await _orderService.CancelOrderAsync(orderId);
            return StatusCode(result.StatusCode, result);
        }
    }
}