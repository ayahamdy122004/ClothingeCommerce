using AutoMapper;
using E_Commerce.Entities.DTO.Models.CART;
using E_Commerce.Entities.Model;

namespace E_Commerce.Profiles
{
    public class CartMappingProfile : Profile
    {
        private const decimal ESTIMATED_SHIPPING = 50.00m;

        public CartMappingProfile()
        {
            // Mapping من Request DTO إلى Entity
            CreateMap<AddCartDTO, CartItem>();

            // Mapping من Entities إلى Response DTOs
            CreateMap<CartItem, CartItemResponseDTO>()
                .ForMember(dest => dest.LineTotal, opt => opt.MapFrom(src => src.UnitPrice * src.Quantity));

            CreateMap<Cart, CustomerCartResponseDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.TotalUnits, opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity)))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.Items.Sum(i => i.UnitPrice * i.Quantity)))
                .ForMember(dest => dest.EstimatedShipping, opt => opt.MapFrom(src => src.Items.Any() ? ESTIMATED_SHIPPING : 0))
                .ForMember(dest => dest.EstimatedFinalTotal, opt => opt.MapFrom(src => src.Items.Any() ? (src.Items.Sum(i => i.UnitPrice * i.Quantity) + ESTIMATED_SHIPPING) : 0));
        }
    }
}