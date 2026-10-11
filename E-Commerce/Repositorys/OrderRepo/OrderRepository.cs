using ClothingStore.Entities;
using E_Commerce.Entities.Data;
using E_Commerce.Entities.DTO.Models.ORDER;
using E_Commerce.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_Commerce.Repositorys.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        #region fields
        private readonly AppDbContext _db;
        #endregion

        #region constructor
        public OrderRepository(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Actions
        public async Task<Order> AddOrderAsync(Order order)
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            return order;
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserEmail(string email)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.CustomerEmail == email)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<IEnumerable<Order>> GetOrders(string userId)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.CustomerId == userId)
                .OrderByDescending(o => o.OrderDate)
               // .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Order> UpdateOrderAsync(Order order)
        {

            var local = await _db.Orders.FirstOrDefaultAsync(x=>x.Id==order.Id);

            if(local==null)
                return null;

         
                 _db.Orders.Update(order);
            await SaveChanges();

           return order;        
        }  
        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
        public Task<Order?> GetOrderForUpdateAsync(int id) =>
           _db.Orders
              .Include(o => o.OrderItems)
              .FirstOrDefaultAsync(o => o.Id == id);   // من غير AsNoTracking

        public async Task<IDbContextTransaction> BeginTransactionAsync() =>
    await _db.Database.BeginTransactionAsync();

        public Task<ProductVariation?> GetVariationWithProductAsync(int id) =>
            _db.ProductVariations.Include(v => v.Product).FirstOrDefaultAsync(v => v.Id == id);

        public async Task<bool> TryDecreaseStockAsync(int variationId, int quantity) =>
            await _db.ProductVariations
                .Where(v => v.Id == variationId && v.StockQuantity >= quantity)
                .ExecuteUpdateAsync(s => s.SetProperty(v => v.StockQuantity, v => v.StockQuantity - quantity)) > 0;

        public Task IncreaseStockAsync(int ?variationId, int quantity) =>
            _db.ProductVariations
                .Where(v => v.Id == variationId)
                .ExecuteUpdateAsync(s => s.SetProperty(v => v.StockQuantity, v => v.StockQuantity + quantity));
        #endregion

    }
    
}