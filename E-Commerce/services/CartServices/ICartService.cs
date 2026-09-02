using E_Commerce.Entities.DTO.Models.CART;
using E_Commerce.Entities.Model;

namespace E_Commerce.services.CartServices
{
    public interface ICartService
    {
        CustomerCartResponseDTO GetCart();
        void AddToCartAsync(AddCartDTO item);
        CustomerCartResponseDTO UpdateQuantityAsync(UpdateCartDTO model);
        void RemoveItemAsync(int productId);
        void ClearCartAsync();
    }
}