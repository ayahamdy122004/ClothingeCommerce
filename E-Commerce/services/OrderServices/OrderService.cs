using AutoMapper;
using Azure;
using ClothingStore.Entities;
using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Repositorys.OrderRepo;
using E_Commerce.Repositorys.ProductRepo;
using E_Commerce.Repositorys.VariationRepo;
using E_Commerce.services.CartServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

            IHttpContextAccessor httpContextAccessor, 
        }
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
            }
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
                }
                {
                    {
                        StatusCode = 400,
                    };
                }
                
                var orderItems = new List<OrderItem>();
                {
                    {
                        {
                            StatusCode = 400,
                        };
                    }
                   
                    if (variation == null)
                    {
                    }

                    if (variation.StockQuantity < item.Quantity)
                    {
                    }

                    variation.StockQuantity -= item.Quantity;

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
                decimal finalTotal = subtotal + shippingCost;
                var newOrder = new Order
                {
                    OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}",
                    CustomerId = userId,
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


            {
                };
            }
        {
            var userId = GetUserId();
            {
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Order not found or you are not authorized to cancel this order."
                };
            }
            {
                {
                    StatusCode = 400,
                };
            }
            {
                {
                    StatusCode = 400,
                };
            }

            order.OrderStatus = "Cancelled";
            order.UpdatedAt = DateTime.UtcNow;


        {
            if (order == null)
            {
                {
                };
            }


        }
        {
            if (userExists == null)
            {
            }


            {
        }
        {
            if (order == null)
            {
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Order not found."
                };
            }

            order.OrderStatus = newStatus;
            order.UpdatedAt = DateTime.UtcNow;

        }

    

        

        private string GetUserId()
        {
                ?? string.Empty;
        }

      
    }
}