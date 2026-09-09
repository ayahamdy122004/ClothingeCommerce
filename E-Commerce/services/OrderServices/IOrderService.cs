using E_Commerce.Entities.DTO.Models.ORDER;

namespace E_Commerce.services.OrderServices
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderResponseDTO>> GetOrders(string userId);
        Task<OrderResponseDTO> CancelOrderAsync(int orderId);
        public Task<OrderResponseDTO> GetOrderById(int id)
;
        // public Task<OrderResponseDTO> UpdateOrder(OrderItemResponseDTO order);
        Task<OrderResponseDTO> UpdateOrderStatusAsync(int orderId, string newStatus);
        public Task<OrderResponseDTO> AddOrder(CheckoutDTO order);

    }
}
