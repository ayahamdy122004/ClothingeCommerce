using E_Commerce.Entities.DTO.Models.CART;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.services.CartServices
{
    public interface ICartService
    {
        Task<ApiResponse<CustomerCartResponseDTO>> GetCartAsync();
        Task<ApiResponse<CustomerCartResponseDTO>> AddToCartAsync(AddCartDTO item);
        Task<ApiResponse<CustomerCartResponseDTO>> UpdateQuantityAsync(UpdateCartDTO model);
        Task<ApiResponse<CustomerCartResponseDTO>> RemoveItemAsync(int productVariationId);
        Task<ApiResponse<bool>> ClearCartAsync();
    }
}