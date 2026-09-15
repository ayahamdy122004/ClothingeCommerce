using AutoMapper;
using ClothingStore.Entities;
using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model;
using E_Commerce.Repositorys.OrderRepo;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<OrderResponseDTO>> AddOrder(CheckoutDTO orderDto)
        {
            if (orderDto == null)
            {
                return ApiResponse<OrderResponseDTO>.FailureResponse(
                    "Invalid order data",
                    StatusCodes.Status400BadRequest);
            }

            var orderEntity = _mapper.Map<Order>(orderDto);
            var createdOrder =  _orderRepository.AddOrderAsync(orderEntity);

            var resultDto = _mapper.Map<OrderResponseDTO>(createdOrder);
            return ApiResponse<OrderResponseDTO>.SuccessResponse(
                resultDto,
                "Order created successfully",
                StatusCodes.Status201Created);
        }
        public async Task<ApiResponse<IEnumerable<OrderResponseDTO>>> GetOrders(string userId)
        {
            // استدعاء GetOrders بدلاً من GetOrderByIdAsync
            var orders = await _orderRepository.GetOrders(userId);

            if (orders == null || !orders.Any())
            {
                return ApiResponse<IEnumerable<OrderResponseDTO>>.FailureResponse(
                    "No orders found for this user",
                    StatusCodes.Status404NotFound);
            }

            var ordersDto = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
            return ApiResponse<IEnumerable<OrderResponseDTO>>.SuccessResponse(
                ordersDto,
                "Orders retrieved successfully",
                StatusCodes.Status200OK);
        }
        public async Task<ApiResponse<OrderResponseDTO>> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return ApiResponse<OrderResponseDTO>.FailureResponse(
                    $"Order with ID {id} not found",
                    StatusCodes.Status404NotFound);
            }

            var orderDto = _mapper.Map<OrderResponseDTO>(order);
            return ApiResponse<OrderResponseDTO>.SuccessResponse(
                orderDto,
                "Order retrieved successfully",
                StatusCodes.Status200OK);
        }

        public async Task<ApiResponse<OrderResponseDTO>> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return ApiResponse<OrderResponseDTO>.FailureResponse(
                    $"Order with ID {orderId} not found",
                    StatusCodes.Status404NotFound);
            }

            order.OrderStatus = newStatus;
            await _orderRepository.UpdateOrderAsync(order);

            var orderDto = _mapper.Map<OrderResponseDTO>(order);
            return ApiResponse<OrderResponseDTO>.SuccessResponse(
                orderDto,
                "Order status updated successfully",
                StatusCodes.Status200OK);
        }
    }
}