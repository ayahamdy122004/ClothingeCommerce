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
  
    public class OrderController : ControllerBase
    {
        #region
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #endregion
        #region
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrderResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        #endregion
      
        #region
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        #endregion

        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderById(id);

            if (result==null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        #region
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        #endregion

        public async Task<IActionResult> CreateOrder([FromBody] CheckoutDTO orderDto)
        {
            var result = await _orderService.AddOrder(orderDto);

            if (result == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        #region
        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        #endregion
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string newStatus)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, newStatus);

            if (result == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        [HttpGet("status/{orderId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DisplayStatusOrder(int orderId)
        {
            var result = await _orderService.DisplayStatusOrder(orderId);
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("DisplayShippmentOrder/{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DisplayShippmentOrder(int orderId)
        {
            var result = await _orderService.DisplayShippmentOrder(orderId);
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("GetOrdersByUserEmail/{email}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrderResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrdersByemailUser(string email)
        {
            var result = await _orderService.GetOrdersByUserEmail(email);    
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("CancelledOrder/{Id}")]
        [ProducesResponseType(typeof(ApiResponse<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelOrder(int Id)
        {
            var result = await _orderService.CancelOrderAsync(Id);
            if (result == null)
            {
                return NotFound(result);
            }
            else if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}