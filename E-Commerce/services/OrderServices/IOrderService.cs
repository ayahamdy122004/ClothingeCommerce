using E_Commerce.Entities.DTO.Models.ORDER;

namespace E_Commerce.services.OrderServices
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderResponseDTO>> GetOrders(string userId);
        public Task<OrderResponseDTO> GetOrderById(int id)
;
        public Task<OrderResponseDTO> UpdateOrder(OrderItemResponseDTO order);
        public Task<OrderItemResponseDTO> AddOrder(CheckoutDTO order);

    }
}
