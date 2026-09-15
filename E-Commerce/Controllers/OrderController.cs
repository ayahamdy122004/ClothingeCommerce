using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.services.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrderResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _orderService.GetOrders(userId);

            if (result.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderById(id);

            if (result.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CheckoutDTO orderDto)
        {
            var result = await _orderService.AddOrder(orderDto);

            if (result.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(result);
            }

            return StatusCode(StatusCodes.Status201Created, result);
        }
        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string newStatus)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, newStatus);

            if (result.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(result);
            }

            return Ok(result);
        }


    }
}