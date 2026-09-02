using AutoMapper;
using E_Commerce.Entities.DTO.Models.CART;
using E_Commerce.Entities.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace E_Commerce.services.CartServices
{
    public class CartService : ICartService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeSpan _cartExpiration = TimeSpan.FromHours(72);

        public CartService(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return userId;
        }

        public string GetCacheKey(string userId) => $"cart_{userId}";

        public CustomerCartResponseDTO GetCart()
        {
            var userId = GetUserId();
            var cacheKey = GetCacheKey(userId);

            if (!_memoryCache.TryGetValue(cacheKey, out Cart? cart) || cart == null)
            {
                cart = new Cart();
                _memoryCache.Set(cacheKey, cart, _cartExpiration);
            }

            return _mapper.Map<CustomerCartResponseDTO>(cart);
        }

        public void AddToCartAsync(AddCartDTO item)
        {
            var userId = GetUserId();
            var cacheKey = GetCacheKey(userId);

            if (!_memoryCache.TryGetValue(cacheKey, out Cart? cart) || cart == null)
            {
                cart = new Cart();
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductVariationId == item.ProductVariationId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                var newItem = _mapper.Map<CartItem>(item);
                cart.Items.Add(newItem);
            }

            _memoryCache.Set(cacheKey, cart, _cartExpiration);
        }

        public CustomerCartResponseDTO UpdateQuantityAsync(UpdateCartDTO model)
        {
            var userId = GetUserId();
            var cacheKey = GetCacheKey(userId);

            if (!_memoryCache.TryGetValue(cacheKey, out Cart? cart) || cart == null)
            {
                cart = new Cart();
            }

            var item = cart.Items.FirstOrDefault(i => i.ProductId == model.ProductId);

            if (item != null)
            {
                if (model.Quantity <= 0)
                {
                    cart.Items.Remove(item);
                }
                else
                {
                    item.Quantity = model.Quantity;
                }

                _memoryCache.Set(cacheKey, cart, _cartExpiration);
            }

            return _mapper.Map<CustomerCartResponseDTO>(cart);
        }

        public void RemoveItemAsync(int productId)
        {
            var userId = GetUserId();
            var cacheKey = GetCacheKey(userId);

            if (_memoryCache.TryGetValue(cacheKey, out Cart? cart) && cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    _memoryCache.Set(cacheKey, cart, _cartExpiration);
                }
            }
        }

        public void ClearCartAsync()
        {
            var userId = GetUserId();
            _memoryCache.Remove(GetCacheKey(userId));
        }
    }
}