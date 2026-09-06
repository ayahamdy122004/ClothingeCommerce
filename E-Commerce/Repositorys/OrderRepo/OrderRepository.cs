using ClothingStore.Entities;
using E_Commerce.Entities.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositorys.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext db;
        public OrderRepository(AppDbContext db) { 
            this.db = db;   
        }   
        public async Task AddOrderAsync(Order order)
        {
            await db.Orders.AddAsync(order);
            SaveChanges();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await db.Orders
                   .Include(o => o.OrderItems) 
                   .AsNoTracking()
                   .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrders(string userId)
        {
            return await db.Orders
                .Include(o => o.OrderItems) 
                .Where(o => o.CustomerId == userId) 
                .OrderByDescending(o => o.OrderDate) 
                .AsNoTracking() 
                .ToListAsync();
        }

        //public async Task SaveChanges()
        //{
        //   await db.SaveChanges();
        //}
        public async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        { var orderItem = db.Orders.FirstOrDefault(i => i.Id == order.Id);
            if (orderItem != null)
            {
                db.Orders.Update(order);
                SaveChanges();
            }
           
        }
    }
}
