using AutoMapper;
using ClothingStore.Entities;
using E_Commerce.Entities.DTO.Models.PRODUCTS;
using E_Commerce.Entities.Model;

namespace E_Commerce.services.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // 1. من CreateProductRequestDTO إلى Product Entity
            CreateMap<CreateProductRequestDTO, Product>();

            // 2. من UPdateProductRequestDTO إلى Product Entity
            CreateMap<UPdateProductRequestDTO, Product>();

            // 3. من Product Entity إلى ProductResponseDTO (تلقائي 100%)
            CreateMap<Product, ProductResponseDTO>();

            // 4. من Product Entity إلى ProductListResponseDTO (تلقائي مع الحسابات الخاصة)
            CreateMap<Product, ProductListResponseDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src => src.CoverImageUrl))
                .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => src.DiscountPrice.HasValue ? src.DiscountPrice.Value : src.BasePrice))
                .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.DiscountPrice.HasValue ? src.BasePrice : (decimal?)null))
                .ForMember(dest => dest.AvailableColors, opt => opt.MapFrom(src => src.Variations != null
                    ? src.Variations.Where(v => v.IsActive).Select(v => v.Color).Distinct().ToList()
                    : new List<string>()))
                .ForMember(dest => dest.AvailableSizes, opt => opt.MapFrom(src => src.Variations != null
                    ? src.Variations.Where(v => v.IsActive).Select(v => v.Size).Distinct().ToList()
                    : new List<string>()))
                .ForMember(dest => dest.InStockStatus, opt => opt.MapFrom(src => src.Variations != null && src.Variations.Any(v => v.IsActive && v.StockQuantity > 0)
                    ? "In Stock"
                    : "Out of Stock"));
            CreateMap<Product, ProductDetailsResponseDTO>()
          .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src => src.CoverImageUrl));
        }
    }
}