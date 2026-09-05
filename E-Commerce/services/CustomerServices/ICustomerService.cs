using E_Commerce.Entities.DTO.CUSTOMER;

namespace E_Commerce.services.CustomerServices
{
    public interface ICustomerService
    {
        public Task<UserProfileResponseDTO> GetCustomer(string email);
        public Task<UserProfileResponseDTO> UpdateCustomer(string email, UpdateUserProfileDTO customer);
    }
}
