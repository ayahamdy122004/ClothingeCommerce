using ClothingStore.Entities;
using E_Commerce.Entities.Data;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Repositorys.CustomerRepo
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly UserManager<ApplicationUser> user;
        private readonly AppDbContext db;
        public CustomerRepository(AppDbContext db, UserManager<ApplicationUser> user)
        {
            this.db = db;
            this.user = user;
        }

        public async Task<ApplicationUser> GetCustomer(string email)
        {
           var customer = await user.FindByEmailAsync(email);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }
            return customer;
        }

        public async Task<bool> UpdateCustomer(ApplicationUser customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;

            var result = await user.UpdateAsync(customer);
            return result.Succeeded;
        }


    }
}
