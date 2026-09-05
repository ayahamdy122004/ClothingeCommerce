using ClothingStore.Entities;

namespace E_Commerce.Repositorys.CustomerRepo
{
    public interface ICustomerRepository
    {
        public Task<ApplicationUser> GetCustomer(string email);
//        public Task<ApplicationUser> GetCustomers();

        public Task<bool> UpdateCustomer(ApplicationUser customer);

    }
}
