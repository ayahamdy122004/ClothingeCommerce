using AutoMapper;
using E_Commerce.Entities.DTO.Models.BRANDS;
using E_Commerce.Entities.Model;

namespace E_Commerce.Profiles
{
    public class BrandProfile:Profile
    {
        public BrandProfile()
        {
            CreateMap<BrandResponseDTO, Brand>();
        }
    }
}
