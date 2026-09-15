using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.services.OrderServices
{
    public interface IOrderService
    {
        Task<ApiResponse<IEnumerable<OrderResponseDTO>>> GetOrders(string userId);
        Task<ApiResponse<OrderResponseDTO>> AddOrder(CheckoutDTO orderDto);
        Task<ApiResponse<OrderResponseDTO>> GetOrderById(int id);
        Task<ApiResponse<OrderResponseDTO>> UpdateOrderStatusAsync(int orderId, string newStatus);
      //  Task<ApiResponse<OrderResponseDTO>> CancelOrderAsync(int orderId);
    }
}