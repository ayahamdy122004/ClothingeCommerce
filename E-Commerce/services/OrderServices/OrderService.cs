using AutoMapper;
using ClothingStore.Entities;    // ApplicationUser
using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Enums;
using E_Commerce.Entities.Model; // Entities الخاصّة بـ Order و Cart
using E_Commerce.Repositorys.OrderRepo;
using E_Commerce.services.CartServices;
using Jose;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace E_Commerce.services.OrderServices
{
    public class OrderService : IOrderService
    {
        #region
        private readonly IOrderRepository _orderRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ICartService cart;

        public OrderService(
            IOrderRepository orderRepository,
            IMemoryCache memoryCache,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper, ICartService cart)
        {
            _orderRepository = orderRepository;
            _memoryCache = memoryCache;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            this.cart = cart;
            
        }
        private string GetUserId()
        {

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("uid")?.Value
                      ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return userId;
        }

        private string GetCacheKey(string key) => $"cart:user:{key}";
        #endregion
        public async Task<ApiResponse<OrderResponseDTO>> AddOrder(CheckoutDTO orderDto)
        {
            if (orderDto == null)
            {
                return ApiResponse<OrderResponseDTO>.FailureResponse("Invalid order data", 400);
            }
            var userIdOrEmail = GetUserId();
            var user = await _userManager.FindByIdAsync(userIdOrEmail)
                    ?? await _userManager.FindByEmailAsync(userIdOrEmail);

            if (user == null)
            {
                return ApiResponse<OrderResponseDTO>.FailureResponse("User not found in database.", 404);
            }
            string keyByEmail = GetCacheKey(user.Email);
            string keyById = GetCacheKey(user.Id);

            Cart? cart = null;
            bool isCartFound = _memoryCache.TryGetValue(keyByEmail, out cart) ||
                               _memoryCache.TryGetValue(keyById, out cart);

            if (!isCartFound || cart == null || !cart.Items.Any())
            {
                return ApiResponse<OrderResponseDTO>.FailureResponse("Your cart is empty. Please add items before checking out.", 400);
            }

            var orderEntity = _mapper.Map<Order>(orderDto);
            orderEntity.CustomerId = user.Id;
            orderEntity.CustomerFirstName = user.FirstName;
            orderEntity.CustomerLastName = user.LastName;
            orderEntity.OrderNumber =
$"ORD-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
            orderEntity.PaymentMethod = "CashOnDelivery";
            orderEntity.PaymentStatus = " Unpaidش";
            orderEntity.CustomerEmail = user.Email;

            orderEntity.OrderItems = cart.Items.Select(ci => new OrderItem
            {
                ProductVariationId = ci.ProductVariationId,

                Quantity = ci.Quantity,
                UnitPrice = ci.UnitPrice
                ,
                SelectedColor = ci.SelectedColor,
                SelectedSize = ci.SelectedSize,
                ProductId = ci.ProductId
                ,
                ProductName = ci.ProductName

            }).ToList();
            orderEntity.FinalTotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);
            await _orderRepository.AddOrderAsync(orderEntity);
            _memoryCache.Remove(keyByEmail);
            _memoryCache.Remove(keyById);
            var resultDto = _mapper.Map<OrderResponseDTO>(orderEntity);
         //   await cart.ClearCartAsync();
            return ApiResponse<OrderResponseDTO>.SuccessResponse(resultDto, "Order created successfully.", 201);
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
        public async Task<ApiResponse<String>> DisplayStatusOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return ApiResponse<String>.FailureResponse($"Order with ID {orderId} not found", 404);

            }
            var d = order.OrderStatus;
            return ApiResponse<String>.SuccessResponse(d, "Order status retrieved successfully", 200);

        }

        public async Task<ApiResponse<string>> DisplayShippmentOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return ApiResponse<string>.FailureResponse($"Order {orderId} does not exit", 400);
            var x = order.ShipmentStatus;
            return ApiResponse<string>.SuccessResponse(x, "Order retrieved shipment Status Succesifully", 200);
        }

        public async Task<ApiResponse<IEnumerable<OrderResponseDTO>>> GetOrdersByUserEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return ApiResponse<IEnumerable<OrderResponseDTO>>.FailureResponse($"user {email} does not exit", 400);
            var x = await _orderRepository.GetOrdersByUserEmail(email);
            var orderDtos = _mapper.Map<IEnumerable<OrderResponseDTO>>(x);
            return ApiResponse<IEnumerable<OrderResponseDTO>>.SuccessResponse(orderDtos, $"All Order For {email} retrieved sucessfully", 200);

        }

        public async Task<ApiResponse<OrderResponseDTO>> CancelOrderAsync(int orderId)
        {
           var x= await _orderRepository.GetOrderByIdAsync(orderId);
            if(x==null)
            {
                return ApiResponse<OrderResponseDTO>.
                    FailureResponse($"Order with ID {orderId} not found", 404);
            }
            var orderStatus = x.OrderStatus;
            if(orderStatus == "Pending" || orderStatus == "Confirmed")
            {
                x.OrderStatus = "Cancelled";
                await _orderRepository.UpdateOrderAsync(x);
                var y=_mapper.Map<OrderResponseDTO>(x);
                return ApiResponse<OrderResponseDTO>.
                    SuccessResponse(y,$"Order with ID {orderId} is  cancelled Successfully", 200);
            }
            else
            {                 return ApiResponse<OrderResponseDTO>.
                    FailureResponse($" For this Order this Operater", 400);
            }
        }
    }
}