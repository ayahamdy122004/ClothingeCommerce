using ClothingStore.Entities;
using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.Model;
using Microsoft.EntityFrameworkCore.Storage; // أو ClothingStore.Entities حسب النيم سبيس عندك

namespace E_Commerce.Repositorys.OrderRepo
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrders(string userId);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<ProductVariation?> GetVariationWithProductAsync(int id);
        Task<bool> TryDecreaseStockAsync(int variationId, int quantity);
        Task IncreaseStockAsync(int ?variationId, int quantity);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order> UpdateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserEmail(string email);
        Task SaveChanges();
        // Task AddOrderAsync(CheckoutDTO order);
        Task<Order?> GetOrderForUpdateAsync(int id);
    }
}