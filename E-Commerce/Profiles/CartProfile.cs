using AutoMapper;
using E_Commerce.Entities.Model;
using E_Commerce.Entities.DTO.Models.CART;

namespace E_Commerce.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            // التحويل من CartItem (Model) إلى CartItemResponseDTO
            CreateMap<CartItem, CartItemResponseDTO>();

            // التحويل من AddCartDTO إلى CartItem (عند الإضافة للـ Cache)
            CreateMap<AddCartDTO, CartItem>();

            // التحويل من Cart (Model) إلى CustomerCartResponseDTO
            CreateMap<Cart, CustomerCartResponseDTO>();
        }
    }
}