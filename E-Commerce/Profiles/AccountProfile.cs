using AutoMapper;
using ClothingStore.Entities;

namespace E_Commerce.Profiles
{
    public class AccountProfile:Profile
    {
        public AccountProfile()
        {
            CreateMap<UpdateProfile,ApplicationUser>();
        }
    }
}
