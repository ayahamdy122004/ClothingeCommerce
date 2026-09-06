using AutoMapper;
using ClothingStore.Entities;
using E_Commerce.Entities.DTO.Models.ORDER;

namespace E_Commerce.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // 1. الماب الخاص بعناصر الأوردر
            CreateMap<OrderItem, OrderItemResponseDTO>();

            // 2. الماب الأساسي للأوردر
            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.FinalTotal))

                // دمج اسم العميل الأول والأخير
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src =>
                    $"{src.CustomerFirstName} {src.CustomerLastName}".Trim()))

                // تجميع عنوان الشحن كاملاً من الفيلدز المتاحة
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src =>
                    $"{src.Street}, {src.City}, {src.Governorate}, {src.Country}".Replace(", ,", ",")))

                // ربط قائمة المنتجات
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}