using AutoMapper;
using ClothingStore.Entities;
using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Repositorys.OrderRepo;
using E_Commerce.Repositorys.ProductRepo;
using E_Commerce.Repositorys.VariationRepo;
using E_Commerce.services.CartServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace E_Commerce.services.OrderServices
{
    public class OrderService : IOrderService
    {
        #region Fields
        private readonly IOrderRepository _repo;
        private readonly ICartService _cart;
        private readonly IProductRepository _prod;
        private readonly IVariationRepository _variationrepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderService(
            IOrderRepository repo,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            ICartService cart,
            IProductRepository prod,
            IVariationRepository variationrepo)
        {
            _prod = prod;
            _cart = cart;
            _mapper = mapper;
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _variationrepo = variationrepo;
        }
  #endregion
        public async Task<ApiResponse<OrderResponseDTO>> AddOrder(CheckoutDTO checkoutDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new ApiResponse<OrderResponseDTO>
                {
                    StatusCode = 401,
                    Success = false,
                    Message = "Customer is not authenticated."
                };
            }
            var userExists = await _userManager.FindByIdAsync(userId);
            if (userExists == null || !userExists.EmailConfirmed)
            {
                return new ApiResponse<OrderResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "User not found or email not confirmed."
                };
            }

            if (string.IsNullOrWhiteSpace(checkoutDto.Street) ||
                string.IsNullOrWhiteSpace(checkoutDto.City) ||
                string.IsNullOrWhiteSpace(checkoutDto.Country))
            {
                return new ApiResponse<OrderResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Required shipping-address fields are missing."
                };
            }

            var cartData = _cart.GetCart();
            if (cartData == null || !cartData.Items.Any())
            {
                return new ApiResponse<OrderResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "The cart is empty. Checkout is not available."
                };
            }

            var orderItems = new List<OrderItem>();
            foreach (var item in cartData.Items)
            {
                var product = await _prod.GetByIdAsync(item.ProductId);
                if (product == null || product.IsActive != true)
                {
                    return new ApiResponse<OrderResponseDTO>
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = $"The product with ID {item.ProductId} does not exist or is not active."
                    };
                }

                var variation = await _variationrepo.GetById(item.ProductVariationId);
                if (variation == null)
                {
                    return new ApiResponse<OrderResponseDTO>
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = $"Variation with ID {item.ProductVariationId} not found."
                    };
                }

                if (variation.StockQuantity < item.Quantity)
                {
                    return new ApiResponse<OrderResponseDTO>
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = $"Not enough stock for product {item.ProductName}."
                    };
                }

                variation.StockQuantity -= item.Quantity;
                decimal actualPrice = (product.DiscountPrice.HasValue && product.DiscountPrice.Value > 0)
                    ? product.DiscountPrice.Value
                    : product.BasePrice;

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductVariationId = item.ProductVariationId,
                    ProductName = item.ProductName,
                    SelectedColor = item.SelectedColor,
                    SelectedSize = item.SelectedSize,
                    UnitPrice = actualPrice,
                    Quantity = item.Quantity,
                    CoverImageUrl = item.CoverImageUrl
                });
            }

            decimal subtotal = orderItems.Sum(x => x.UnitPrice * x.Quantity);
            decimal shippingCost = 50;
            decimal finalTotal = subtotal + shippingCost;

            var newOrder = new Order
            {
                OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}",
                CustomerId = userId,
                CustomerFirstName = userExists.FirstName ?? string.Empty,
                CustomerLastName = userExists.LastName ?? string.Empty,
                CustomerEmail = userExists.Email ?? string.Empty,
                CustomerPhoneNumber = userExists.PhoneNumber,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Pending",
                ShipmentStatus = "NotPrepared",
                PaymentMethod = string.IsNullOrEmpty(checkoutDto.PaymentMethod) ? "CashOnDelivery" : checkoutDto.PaymentMethod,
                PaymentStatus = "Unpaid",

                Subtotal = subtotal,
                ShippingCost = shippingCost,
                FinalTotal = finalTotal,

                Street = checkoutDto.Street,
                City = checkoutDto.City,
                Governorate = checkoutDto.State,
                Country = checkoutDto.Country,
                PostalCode = checkoutDto.ZipCode,

                OrderItems = orderItems
            };

            await _repo.AddOrderAsync(newOrder);
             _cart.ClearCartAsync();

            var mappedOrder = _mapper.Map<OrderResponseDTO>(newOrder);

            return new ApiResponse<OrderResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "The order has been successfully created.",
                Data = mappedOrder
            };
        }
        public async Task<ApiResponse<OrderResponseDTO>> CancelOrderAsync(int orderId)
        {
            var userId = GetUserId();
            var order = await _repo.GetOrderByIdAsync(orderId);

            if (order == null || order.CustomerId != userId)
            {
                return new ApiResponse<OrderResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Order not found or you are not authorized to cancel this order."
                };
            }

            if (order.OrderStatus == "Cancelled")
            {
                return new ApiResponse<OrderResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Order is already cancelled."
                };
            }

            if (order.OrderStatus != "Pending" && order.OrderStatus != "Processing")
            {
                return new ApiResponse<OrderResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Order cannot be cancelled at this stage."
                };
            }

            order.OrderStatus = "Cancelled";
            order.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateOrderAsync(order);

            var mappedOrder = _mapper.Map<OrderResponseDTO>(order);

            return new ApiResponse<OrderResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "Order has been cancelled successfully.",
                Data = mappedOrder
            };
        }
        public async Task<ApiResponse<OrderResponseDTO>> GetOrderById(int id)
        {
            var order = await _repo.GetOrderByIdAsync(id);
            if (order == null)
            {
                return new ApiResponse<OrderResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "This order doesn't exist."
                };
            }

            var mappedOrder = _mapper.Map<OrderResponseDTO>(order);

            return new ApiResponse<OrderResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "Order retrieved successfully.",
                Data = mappedOrder
            };
        }
        public async Task<ApiResponse<IEnumerable<OrderResponseDTO>>> GetOrders(string userId)
        {
            var userExists = await _userManager.FindByIdAsync(userId);
            if (userExists == null)
            {
                return new ApiResponse<IEnumerable<OrderResponseDTO>>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "User not found."
                };
            }

            var orders = await _repo.GetOrders(userId);
            var mappedOrders = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders ?? Enumerable.Empty<Order>());

            return new ApiResponse<IEnumerable<OrderResponseDTO>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Orders retrieved successfully.",
                Data = mappedOrders
            };
        }
        public async Task<ApiResponse<OrderResponseDTO>> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _repo.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new ApiResponse<OrderResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Order not found."
                };
            }

            order.OrderStatus = newStatus;
            order.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateOrderAsync(order);

            var mappedOrder = _mapper.Map<OrderResponseDTO>(order);

            return new ApiResponse<OrderResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "Order status updated successfully.",
                Data = mappedOrder
            };
        }
        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                ?? string.Empty;
        }
    }
}