using AutoMapper;
using ClothingStore.Entities;
using E_Commerce.Entities.DTO.Models.ORDER;

namespace E_Commerce.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {

            CreateMap<OrderItem, OrderItemResponseDTO>();
            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.FinalTotal))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src =>
                    $"{src.CustomerFirstName} {src.CustomerLastName}".Trim()))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src =>
                    $"{src.Street}, {src.City}, {src.Governorate}, {src.Country}".Replace(", ,", ",")))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<CheckoutDTO, Order>();
        }
    }
}