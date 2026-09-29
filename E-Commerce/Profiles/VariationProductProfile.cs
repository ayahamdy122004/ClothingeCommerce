using AutoMapper;
using E_Commerce.Entities.DTO.Models.Variation;
using E_Commerce.Entities.Model;

namespace E_Commerce.Profiles
{
    public class VariationProductProfile : Profile
    {
        public VariationProductProfile()
        {
            CreateMap<CreateVariationProductDTO, ProductVariation>();
            CreateMap<ProductVariation, VariationProductResponseDTO>();
            CreateMap<UpdateVariationProductDTO, ProductVariation>();
          //  CreateMap< ProductVariation, UpdateVariationProductDTO>();
        }
        }
    }
