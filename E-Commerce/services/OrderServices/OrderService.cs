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

            Cart? cachedCart = null;                                          // اتغير الاسم من cart
            bool isCartFound = _memoryCache.TryGetValue(keyByEmail, out cachedCart) ||
                               _memoryCache.TryGetValue(keyById, out cachedCart);

            if (!isCartFound || cachedCart == null || !cachedCart.Items.Any())
            {
                return ApiResponse<OrderResponseDTO>.FailureResponse("Your cart is empty. Please add items before checking out.", 400);
            }

            var orderEntity = _mapper.Map<Order>(orderDto);
            orderEntity.CustomerId = user.Id;
            orderEntity.CustomerFirstName = user.FirstName;
            orderEntity.CustomerLastName = user.LastName;
            orderEntity.OrderNumber = $"ORD-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
            orderEntity.CustomerEmail = user.Email;
            orderEntity.OrderStatus = "Pending";                              // جديد
            orderEntity.ShipmentStatus = "NotPrepared";                       // جديد
            orderEntity.PaymentMethod = "CashOnDelivery";
            orderEntity.PaymentStatus = "Unpaid";                             // اتصلح (كان فيه مسافة)

            // ====== جديد: Transaction + تحقق + خصم المخزون ======
            await using var tx = await _orderRepository.BeginTransactionAsync();

            var items = new List<OrderItem>();
            foreach (var ci in cachedCart.Items)
            {
                var v = await _orderRepository.GetVariationWithProductAsync(ci.ProductVariationId);

                if (v == null || !v.IsActive || !v.Product.IsActive || ci.Quantity <= 0)
                    return ApiResponse<OrderResponseDTO>.FailureResponse($"'{ci.ProductName}' is no longer available.", 400);

                if (!await _orderRepository.TryDecreaseStockAsync(v.Id, ci.Quantity))
                    return ApiResponse<OrderResponseDTO>.FailureResponse($"Not enough stock for '{v.Product.Name}'.", 400);

                items.Add(new OrderItem
                {
                    ProductId = v.ProductId,
                    ProductVariationId = v.Id,
                    ProductName = v.Product.Name,
                    SelectedColor = ci.SelectedColor,
                    SelectedSize = ci.SelectedSize,
                    UnitPrice = ci.UnitPrice,                                      // السعر من الداتابيز
                    Quantity = ci.Quantity
                });
            }

            orderEntity.OrderItems = items;
            orderEntity.FinalTotal = items.Sum(i => i.UnitPrice * i.Quantity);

            await _orderRepository.AddOrderAsync(orderEntity);
            await tx.CommitAsync();                                           // تثبيت كل حاجة
                                                                              // ====================================================

            _memoryCache.Remove(keyByEmail);                                  // بعد النجاح بس
            _memoryCache.Remove(keyById);
            var resultDto = _mapper.Map<OrderResponseDTO>(orderEntity);
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
 public async Task<ApiResponse<OrderResponseDTO>> CancelOrderAsync(int orderId, bool isAdmin = false)
{
    var order = await _orderRepository.GetOrderByIdAsync(orderId);

    if (order == null || (!isAdmin && order.CustomerId != GetUserId()))
        return ApiResponse<OrderResponseDTO>.FailureResponse($"Order with ID {orderId} not found", 404);

    if (order.OrderStatus != "Pending" && order.OrderStatus != "Confirmed")
        return ApiResponse<OrderResponseDTO>.FailureResponse(
            $"Order cannot be cancelled while it is {order.OrderStatus}.", 400);

    await using var tx = await _orderRepository.BeginTransactionAsync();

    foreach (var item in order.OrderItems)
        await _orderRepository.IncreaseStockAsync(item.ProductVariationId, item.Quantity);

    order.OrderStatus = "Cancelled";
    order.PaymentStatus = "Cancelled";
    await _orderRepository.SaveChanges();
    await tx.CommitAsync();

    return ApiResponse<OrderResponseDTO>.SuccessResponse(
        _mapper.Map<OrderResponseDTO>(order), "Order cancelled successfully", 200);
}

        public async Task<ApiResponse<OrderResponseDTO>> UpdateFulfillmentAsync(int id, UpdateOrderFulfillmentDTO dto)
        {
            var shipments = new[] { "NotPrepared", "Preparing", "ReadyForDelivery", "OutForDelivery", "Delivered", "DeliveryFailed", "Returned" };
            var payments = new[] { "Unpaid", "Paid", "Failed", "Refunded", "Cancelled" };

            if (dto.ShipmentStatus == null && dto.PaymentStatus == null)
                return ApiResponse<OrderResponseDTO>.FailureResponse("Nothing to update", 400);
            if (dto.ShipmentStatus != null && !shipments.Contains(dto.ShipmentStatus))
                return ApiResponse<OrderResponseDTO>.FailureResponse("Invalid shipment status", 400);
            if (dto.PaymentStatus != null && !payments.Contains(dto.PaymentStatus))
                return ApiResponse<OrderResponseDTO>.FailureResponse("Invalid payment status", 400);

            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
                return ApiResponse<OrderResponseDTO>.FailureResponse("Order not found", 404);

            if (order.OrderStatus == "Cancelled" || order.OrderStatus == "Rejected")
                return ApiResponse<OrderResponseDTO>.FailureResponse(
                    $"Cannot update an order that is {order.OrderStatus}.", 400);

            if (dto.ShipmentStatus != null)
            {
                if (dto.ShipmentStatus == "Delivered" && order.OrderStatus != "Shipped")
                    return ApiResponse<OrderResponseDTO>.FailureResponse(
                        "Order must be Shipped before it can be Delivered.", 400);

                order.ShipmentStatus = dto.ShipmentStatus;
                if (dto.ShipmentStatus == "Delivered") order.OrderStatus = "Delivered";
            }

            if (dto.PaymentStatus != null)
            {
                if (dto.PaymentStatus == "Paid" && order.ShipmentStatus != "Delivered")
                    return ApiResponse<OrderResponseDTO>.FailureResponse("Payment can be Paid only after delivery", 400);
                order.PaymentStatus = dto.PaymentStatus;
            }

            await _orderRepository.SaveChanges();
            return ApiResponse<OrderResponseDTO>.SuccessResponse(
                _mapper.Map<OrderResponseDTO>(order), "Order updated", 200);
        }
    }
}