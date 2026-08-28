using AutoMapper;
using E_Commerce.Entities.DTO.Models.ProductImages;
using E_Commerce.Entities.Model;

namespace E_Commerce.services.Profiles
{
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImage, ProductImageUploadItemDTO>();
        }
    }
}