using ClothingStore.Entities;
using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.Model; // أو ClothingStore.Entities حسب النيم سبيس عندك

namespace E_Commerce.Repositorys.OrderRepo
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrders(string userId);
        Task<Order?> GetOrderByIdAsync(int id);
        Task UpdateOrderAsync(Order order);
        Task SaveChanges();
       // Task AddOrderAsync(CheckoutDTO order);
    }
}