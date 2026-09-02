using AutoMapper;
using ClothingStore.Entities;
using E_Commerce.Entities.DTO.CUSTOMER;

namespace E_Commerce.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
           
            CreateMap<UpdateUserProfileDTO, ApplicationUser>();

       
            CreateMap<ApplicationUser, UserProfileResponseDTO>();
        }
    }
}