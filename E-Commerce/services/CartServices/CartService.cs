using AutoMapper;
using E_Commerce.Entities.DTO.Models.CART;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model;
using E_Commerce.Repositorys.ProductImageRepo;
using E_Commerce.Repositorys.ProductRepo; 
using E_Commerce.Repositorys.VariationRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace E_Commerce.services.CartServices
{
    public class CartService : ICartService
    {
        #region
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productRepository;
        private readonly IVariationRepository _variationRepository;
        private readonly IProductImageRepository image;
        private readonly IMapper _mapper;
        private readonly TimeSpan _cartExpiration = TimeSpan.FromDays(7);
        private const decimal ESTIMATED_SHIPPING = 50.00m; 

        public CartService(
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor,
            IProductRepository productRepository,
            IVariationRepository variationRepository,
            IMapper mapper, IProductImageRepository image)
        {
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _variationRepository = variationRepository;
            _mapper = mapper;
            this.image = image;
        }
        #endregion
        
        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("uid")?.Value
                      ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return userId;
        }
        private string GetCacheKey(string userId) => $"cart:user:{userId}";
        public async Task<ApiResponse<CustomerCartResponseDTO>> GetCartAsync()
        {
            var userId = GetUserId();
            var cacheKey = GetCacheKey(userId);

            if (!_memoryCache.TryGetValue(cacheKey, out Cart? cart) || cart == null)
            {
                cart = new Cart { UserId = userId,
                 
                   
                    LastUpdatedDate = DateTime.UtcNow };
                _memoryCache.Set(cacheKey, cart, _cartExpiration);
            }

            var responseDto = MapToResponseDTO(cart);
            return ApiResponse<CustomerCartResponseDTO>.SuccessResponse(responseDto, "Cart retrieved successfully.", 200);
        }
        public async Task<ApiResponse<CustomerCartResponseDTO>> AddToCartAsync(AddCartDTO item)
        {
            if (item.Quantity <= 0)
            {
                return ApiResponse<CustomerCartResponseDTO>.FailureResponse("Quantity must be greater than zero.", 400);
            }

            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null || !product.IsActive)
            {
                return ApiResponse<CustomerCartResponseDTO>.FailureResponse("Product does not exist or is inactive.", 400);
            }

            var variation = await _variationRepository.GetById(item.ProductVariationId);
            if (variation == null || !variation.IsActive)
            {
                return ApiResponse<CustomerCartResponseDTO>.FailureResponse("Product variation does not exist or is inactive.", 400);
            }

            var img = await image.GetByIdAsync(item.ProductImageId);
            if (img == null)
            {
                return ApiResponse<CustomerCartResponseDTO>.FailureResponse("Product image does not exist.", 400);
            }

            var userId = GetUserId();
            var cacheKey = GetCacheKey(userId);

            if (!_memoryCache.TryGetValue(cacheKey, out Cart? cart) || cart == null)
            {
                cart = new Cart { UserId = userId };
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductVariationId == item.ProductVariationId);
            int totalRequestedQuantity = item.Quantity + (existingItem?.Quantity ?? 0);

            // Check Available Stock
            if (totalRequestedQuantity > variation.StockQuantity)
            {
                return ApiResponse<CustomerCartResponseDTO>.FailureResponse($"Requested quantity exceeds available stock ({variation.StockQuantity}).", 400);
            }

            if (existingItem != null)
            {
                // 1. تحديث الكمية فقط إذا كان العنصر موجوداً من قبل
                existingItem.Quantity = totalRequestedQuantity;
            }
            else
            {
                // 2. إنشاء عنصر جديد تماماً وإضافته للـ Cart فقط إذا لم يكن موجوداً
                decimal unitPrice = (product.DiscountPrice ?? product.BasePrice) + (variation.PriceAdjustment ?? 0m);

                var cartItem = _mapper.Map<CartItem>(item);
                cartItem.ProductName = product.Name;
                cartItem.SelectedColor = variation.Color;
                cartItem.SelectedSize = variation.Size;
                cartItem.UnitPrice = unitPrice;
                cartItem.CoverImageUrl = img.ImageUrl;

                cart.Items.Add(cartItem);
            }

            cart.LastUpdatedDate = DateTime.UtcNow;
            _memoryCache.Set(cacheKey, cart, _cartExpiration);

            return ApiResponse<CustomerCartResponseDTO>.SuccessResponse(MapToResponseDTO(cart), "Item added to cart successfully.", 200);
        }
        public async Task<ApiResponse<CustomerCartResponseDTO>>  UpdateQuantityAsync(UpdateCartDTO model)
        {
            var userId = GetUserId();
            var cacheKey = GetCacheKey(userId);

            if (!_memoryCache.TryGetValue(cacheKey, out Cart? cart) || cart == null)
            {
                return ApiResponse<CustomerCartResponseDTO>.FailureResponse("Cart is empty.", 404);
            }

            var item = cart.Items.FirstOrDefault(i => i.ProductVariationId == model.ProductVariationId);
            if (item == null)
            {
                return ApiResponse<CustomerCartResponseDTO>.FailureResponse("Item not found in cart.", 404);
            }
            if (model.Quantity <= 0)
            {
                cart.Items.Remove(item);
            }
            else
            {
                var variation = await _variationRepository.GetById(model.ProductVariationId);
                if (variation == null || model.Quantity > variation.StockQuantity)
                {
                    return ApiResponse<CustomerCartResponseDTO>.
                        FailureResponse("Quantity exceeds available stock.", 400);
                }

                item.Quantity = model.Quantity;
            }

            cart.LastUpdatedDate = DateTime.UtcNow;
            _memoryCache.Set(cacheKey, cart, _cartExpiration);

            return ApiResponse<CustomerCartResponseDTO>.SuccessResponse(MapToResponseDTO(cart), "Cart updated successfully.", 200);
        }
        public async Task<ApiResponse<CustomerCartResponseDTO>> RemoveItemAsync(int productVariationId)
        {
            var userId = GetUserId();
            var cacheKey = GetCacheKey(userId);
            if (!_memoryCache.TryGetValue(cacheKey, out Cart? cart) || cart == null || !cart.Items.Any())
            {
                return ApiResponse<CustomerCartResponseDTO>.FailureResponse("Cart is empty or does not exist.", 404);
            }
            var item = cart.Items.FirstOrDefault(i => i.ProductVariationId == productVariationId);
            if (item == null)
            {
                return ApiResponse<CustomerCartResponseDTO>.FailureResponse("Item not found in cart.", 404);
            }
            cart.Items.Remove(item);
            cart.LastUpdatedDate = DateTime.UtcNow;
            _memoryCache.Set(cacheKey, cart, _cartExpiration);
            var responseDto = MapToResponseDTO(cart);
            return ApiResponse<CustomerCartResponseDTO>.SuccessResponse(responseDto, "Item removed successfully.", 200);
        }
        public async Task<ApiResponse<bool>> ClearCartAsync()
        {
            var userId = GetUserId();
            _memoryCache.Remove(GetCacheKey(userId));
            return ApiResponse<bool>.SuccessResponse(true, "Cart cleared successfully.", 200);
        }

        private CustomerCartResponseDTO MapToResponseDTO(Cart cart)
        {
            return _mapper.Map<CustomerCartResponseDTO>(cart);
        }
    }
}