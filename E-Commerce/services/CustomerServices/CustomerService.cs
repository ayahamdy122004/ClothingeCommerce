using AutoMapper;
using E_Commerce.Entities.DTO.CUSTOMER;
using E_Commerce.Entities.DTO.Models.PRODUCTIMAGEFolder;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Repositorys.CustomerRepo;

namespace E_Commerce.services.CustomerServices
{
    public class CustomerService : ICustomerService
    {private readonly ICustomerRepository cus;
        private readonly IMapper mapper;
        public CustomerService(ICustomerRepository cus, IMapper mapper)
        {
            this.mapper = mapper;
            this.cus = cus;
        }
        public async Task<ApiResponse<UserProfileResponseDTO>> GetCustomer(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new ApiResponse<UserProfileResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Email must not be null"
                };
            }

            var customer = await cus.GetCustomer(email);

            if (customer == null)
            {
                return new ApiResponse<UserProfileResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "User Not Found"
                };
            }
            var x = mapper.Map<UserProfileResponseDTO>(customer);
         return  new ApiResponse<UserProfileResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "User Found",
                Data = x
            };
      
        }

public async Task<ApiResponse<UserProfileResponseDTO>> UpdateCustomer(string email,UpdateUserProfileDTO customer)
        {
           var c=await cus.GetCustomer(email);
            if (c == null)
            {
                return new ApiResponse<UserProfileResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "user not found"
                };
            }
            var updatedCustomer = mapper.Map(customer, c);
            var result = await cus.UpdateCustomer(updatedCustomer);
            if (!result)
            {
                return new ApiResponse<UserProfileResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Failed to update user profile"
                };
            }
          //  return mapper.Map<ApiResponse<UserProfileResponseDTO>>(updatedCustomer);
          return new ApiResponse<UserProfileResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "User profile updated successfully",
                Data = mapper.Map<UserProfileResponseDTO>(updatedCustomer)
            };
        }
    }
}
