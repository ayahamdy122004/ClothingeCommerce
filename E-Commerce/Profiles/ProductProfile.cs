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
      
            CreateMap<CreateProductRequestDTO, Product>();
            CreateMap<UPdateProductRequestDTO, Product>();
            CreateMap<Product, ProductResponseDTO>();
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
          .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src => src.CoverImageUrl)).
          ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                // ربط اسم الكاتيجوري
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
   CreateMap<Product, ProductDetailsResponseDTO>()
                 
                    .ForMember(dest => dest.CurrentPrice, opt =>
                        opt.MapFrom(src => src.DiscountPrice ?? src.BasePrice))
                    .ForMember(dest => dest.AvailableColors, opt =>
                        opt.MapFrom(src => src.Variations.Select(v => v.Color).Distinct().ToList()))
                    .ForMember(dest => dest.AvailableSizes, opt =>
                        opt.MapFrom(src => src.Variations.Select(v => v.Size).Distinct().ToList()))
                    .ForMember(dest => dest.AdditionalImages, opt =>
                        opt.MapFrom(src => src.Images.Select(img => img.ImageUrl).ToList()))
                    .ForMember(dest => dest.InStockStatus, opt =>
                        opt.MapFrom(src => src.Variations.Any(v => v.StockQuantity > 0) ? "In Stock" : "Out of Stock"));
            
        
    }
    }
}