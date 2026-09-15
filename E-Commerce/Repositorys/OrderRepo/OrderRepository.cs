using ClothingStore.Entities;
using E_Commerce.Entities.Data;
using E_Commerce.Entities.DTO.Models.ORDER;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositorys.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _db.Orders.AddAsync(order);
            await SaveChanges();
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
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            // التحقق مما إذا كان هناك عنصر بنفس الـ Id متتبع بالفعل في الـ DbContext
            var local = _db.Orders.Local.FirstOrDefault(o => o.Id == order.Id);

            if (local != null)
            {
                // تحديث قيم العنصر المتتبع قيمياً لتجنب تعارض الـ Identity Map
                _db.Entry(local).CurrentValues.SetValues(order);
            }
            else
            {
                // إذا لم يكن متتبعاً، يتم ربطه وتحديثه مباشرة
                _db.Orders.Update(order);
            }

            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
    //public async Task UpdateOrderAsync(Order order)
    //{ var orderItem = db.Orders.FirstOrDefault(i => i.Id == order.Id);
    //    if (orderItem != null)
    //    {
    //        db.Orders.Update(order);
    //        SaveChanges();
    //    }

    //}
}