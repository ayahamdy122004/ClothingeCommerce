using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.services.OrderServices
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderResponseDTO>> AddOrder(CheckoutDTO checkoutDto);
        Task<ApiResponse<OrderResponseDTO>> CancelOrderAsync(int orderId);
        Task<ApiResponse<OrderResponseDTO>> GetOrderById(int id);
        Task<ApiResponse<IEnumerable<OrderResponseDTO>>> GetOrders(string userId);
        Task<ApiResponse<OrderResponseDTO>> UpdateOrderStatusAsync(int orderId, string newStatus);

    }
}