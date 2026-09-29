using E_Commerce.Entities.DTO.CUSTOMER;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.services.CustomerServices
{
    public interface ICustomerService
    {
        public Task<ApiResponse<UserProfileResponseDTO>> GetCustomer(string email);
        public Task<ApiResponse<UserProfileResponseDTO>> UpdateCustomer(string email, UpdateUserProfileDTO customer);
    }
}
