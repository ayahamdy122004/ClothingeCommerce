using AutoMapper;
using Azure;
using ClothingStore.Entities;
using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Repositorys.OrderRepo;
using E_Commerce.Repositorys.ProductRepo;
using E_Commerce.Repositorys.VariationRepo;
using E_Commerce.services.CartServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace E_Commerce.services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository repo;

        private readonly ICartService cart;
        private readonly IProductRepository prod;
        private readonly IVariationRepository variationrepo;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        public OrderService(IOrderRepository repo, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, 
            UserManager<ApplicationUser> userManager ,
            ICartService cart, IProductRepository prod, IVariationRepository variationrepo)
        {this.prod = prod;
            this.cart = cart;
            this.mapper = mapper;
            this.repo = repo;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.variationrepo = variationrepo;
        }
        public async Task<OrderResponseDTO> AddOrder(CheckoutDTO checkoutDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new OrderResponseDTO { StatusCode = 401, Message = "Customer is not authenticated." };
            }
            var userExists = await userManager.FindByIdAsync(userId);
            if (userExists != null && userExists.EmailConfirmed)
            {
                if (string.IsNullOrWhiteSpace(checkoutDto.Street) ||
        string.IsNullOrWhiteSpace(checkoutDto.City) ||
        string.IsNullOrWhiteSpace(checkoutDto.Country))
                {
                    return new OrderResponseDTO { StatusCode = 400, Message = "Required shipping-address fields are missing." };
                }
                var c = cart.GetCart();
                if (c == null || !c.Items.Any())
                {
                    return new OrderResponseDTO
                    {
                        StatusCode = 400,
                        Message = "the cart is empty, Checkout is not available."
                    };
                }
                
                var orderItems = new List<OrderItem>();
                foreach (var item in c.Items)
                {
                    var p = await prod.GetByIdAsync(item.ProductId);
                    if (p == null || p.IsActive != true)
                    {
                        return new OrderResponseDTO
                        {
                            StatusCode = 400,
                            Message = $"The product with ID {item.ProductId} does not exist OR Product Is Not Active."
                        };
                    }
                   
                    var variation = await variationrepo.GetById(item.ProductVariationId);

                    if (variation == null)
                    {
                        return new OrderResponseDTO { StatusCode = 400, Message = "Variation not found." };
                    }
                    if (variation.StockQuantity < item.Quantity)
                    {
                        return new OrderResponseDTO { StatusCode = 400, Message = "Not enough stock." };
                    }

                    variation.StockQuantity -= item.Quantity;
                    decimal actualPrice = (p.DiscountPrice.HasValue && p.DiscountPrice.Value > 0)
                        ? p.DiscountPrice.Value
                        : p.BasePrice;

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
                decimal shippingCost = 50; // مصاريف الشحن
                decimal finalTotal = subtotal + shippingCost;
                var user = await userManager.FindByIdAsync(userId);
                var newOrder = new Order
                {
                    OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}",
                    CustomerId = userId,
                    CustomerFirstName = user?.FirstName ?? string.Empty,
                    CustomerLastName = user?.LastName ?? string.Empty,
                    CustomerEmail = user?.Email ?? string.Empty,
                    CustomerPhoneNumber = user?.PhoneNumber,
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
                await repo.AddOrderAsync(newOrder);
                cart.ClearCartAsync();
                var response = mapper.Map<OrderResponseDTO>(newOrder);
                response.StatusCode = 200;
                response.Message = "The request has been successfully created.";

                return response;
            }

            else
            {
                return new OrderResponseDTO
                {
                    StatusCode = 400,
                    Message = "User not found or email not confirmed."
                };
            }
        }

        public async Task<OrderResponseDTO> CancelOrderAsync(int orderId)
        {
            var userId = GetUserId();
            var order = await repo.GetOrderByIdAsync(orderId);
            if(order == null || order.CustomerId != userId)
            {
                return new OrderResponseDTO
                {
                    StatusCode = 404,
                    Message = "Order not found or you are not authorized to cancel this order."
                };
            }
            if(order.OrderStatus!= "Pending"&& order.OrderStatus!= "Processing")
            {
                return new OrderResponseDTO
                {
                    StatusCode = 400,
                    Message = "Order cannot be cancelled at this stage."
                };
            }
            if(order.OrderStatus == "Cancelled")
            {
                return new OrderResponseDTO
                {
                    StatusCode = 400,
                    Message = "Order is already cancelled."
                };
            }
            order.UpdatedAt = DateTime.UtcNow;
            return await UpdateOrderStatusAsync(orderId, "Cancelled");
            var response=mapper.Map<OrderResponseDTO>(order);
            response.StatusCode = 200;
            response.Message = "Order has been cancelled successfully.";

            return response;
        }

        public async Task<OrderResponseDTO> GetOrderById(int id)
        {
            var order = await repo.GetOrderByIdAsync(id);

            if (order == null)
            {
                return new OrderResponseDTO()
                {
                    Message = "This order doesn't exist.",
                    StatusCode = 404
                };
            }

            var response = mapper.Map<OrderResponseDTO>(order);
            response.StatusCode = 200;

            return response;
        }
        public async Task<IEnumerable<OrderResponseDTO>> GetOrders(string userId)
        {

            var userExists = await userManager.FindByIdAsync(userId);
            if (userExists == null)
            {
                throw new KeyNotFoundException("User not found.");

            }

            var orders = await repo.GetOrders(userId);

            if (orders == null || !orders.Any())
            {
                return Enumerable.Empty<OrderResponseDTO>();
            }


            return mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
        }
        public async Task<OrderResponseDTO> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await repo.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new OrderResponseDTO
                {
                    StatusCode = 404,
                    Message = "Order not found."
                };
            }
            order.OrderStatus = newStatus;
            order.UpdatedAt = DateTime.UtcNow;
            await repo.UpdateOrderAsync(order);
            var response = mapper.Map<OrderResponseDTO>(order);
            response.StatusCode = 200;
            response.Message = "Order status updated successfully.";

            return response;
        }

    

        

        private string GetUserId()
        {
            return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                ?? string.Empty;
        }

      
    }
}