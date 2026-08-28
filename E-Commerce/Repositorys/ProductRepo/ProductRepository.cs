using ClothingStore.Entities;
using E_Commerce.Entities.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositorys.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext db;
        public ProductRepository(AppDbContext db)
        {
            this.db = db;
        }
           public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Variations) 
                .Where(p => p.IsActive)     
                .ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
          
            return await db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Product pro)
        {
            await db.Products.AddAsync(pro);
            await db.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            db.Products.Update(product);
            await db.SaveChangesAsync();
        }
        public async Task<bool> IsSlugExistAsync(string slug, int? excludeId = null)
        {
            return await db.Products.AnyAsync(p => p.Slug == slug && (!excludeId.HasValue || p.Id != excludeId.Value));
        }
        public async Task SaveChangesAsync()
        {
             await SaveChangesAsync();
        }
    }
}