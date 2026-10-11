using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.services.OrderServices
{
    public interface IOrderService
    {
        Task<ApiResponse<IEnumerable<OrderResponseDTO>>> GetOrdersByUserEmail(string email);
        Task<ApiResponse<OrderResponseDTO>> AddOrder(CheckoutDTO orderDto);
        Task<ApiResponse<OrderResponseDTO>> GetOrderById(int id);
        Task<ApiResponse<OrderResponseDTO>> UpdateOrderStatusAsync(int orderId, string newStatus);
      // Task<ApiResponse<OrderResponseDTO>> CancelOrderAsync(int orderId);
      Task<ApiResponse<string>> DisplayStatusOrder(int orderId);
      Task<ApiResponse<string>> DisplayShippmentOrder(int orderId);
        Task<ApiResponse<OrderResponseDTO>> UpdateFulfillmentAsync(int id, UpdateOrderFulfillmentDTO dto);
        Task<ApiResponse<OrderResponseDTO>> CancelOrderAsync(int orderId, bool isAdmin = false);   // عدّل القديمة
       // Task<ApiResponse<OrderResponseDTO>> UpdateFulfillmentAsync(int id, UpdateOrderFulfillmentDTO dto);   // جديدة
    }
}