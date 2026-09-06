using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Repositorys.OrderRepo;
using E_Commerce.services.OrderServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders(string userId)
        {
            var orders = await orderService.GetOrders(userId);
            return Ok(orders);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] CheckoutDTO order)
        {
            var createdOrder = await orderService.AddOrder(order);
            return Ok(createdOrder);
        }
        [HttpGet("GetOrderById{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [HttpPut("UpdateOrderStatus{orderId}/status")]   
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
            {
                var updatedOrder = await orderService.UpdateOrderStatusAsync(orderId, newStatus);
                if (updatedOrder == null)
                {
                    return NotFound();
                }
                return Ok(updatedOrder);
        }
        [HttpDelete("CancelOrder{orderId}")]
       

        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var canceledOrder = await orderService.CancelOrderAsync(orderId);
            if (canceledOrder == null)
            {
                return NotFound();
            }
            return Ok(canceledOrder);
        }
        [HttpPut("UpdateOrder{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId,string order)
        {
            var updatedOrder = await orderService.UpdateOrderStatusAsync(orderId, order);
            if (updatedOrder == null)
            {
                return NotFound();
            }
            return Ok(updatedOrder);
        }
    }
}
