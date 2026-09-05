using AutoMapper;
using E_Commerce.Entities.DTO.CUSTOMER;
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
        public async Task<UserProfileResponseDTO> GetCustomer(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            var customer = await cus.GetCustomer(email);

            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

          
            return mapper.Map<UserProfileResponseDTO>(customer);
        }

        public async Task<UserProfileResponseDTO> 
            UpdateCustomer(string email,UpdateUserProfileDTO customer)
        {
           var c=await cus.GetCustomer(email);
            if (c == null)
            {
                throw new Exception("Customer not found");
            }
            var updatedCustomer = mapper.Map(customer, c);
            var result = await cus.UpdateCustomer(updatedCustomer);
            if (!result)
            {
                throw new Exception("Failed to update customer");
            }
            return mapper.Map<UserProfileResponseDTO>(updatedCustomer);
        }
    }
}
