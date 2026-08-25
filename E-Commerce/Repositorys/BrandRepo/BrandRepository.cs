using E_Commerce.Entities;
using E_Commerce.Entities.Data;
using E_Commerce.Entities.Model;
using E_Commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext db;

        public BrandRepository(AppDbContext db)
        {
           this.db=db;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await db.Brands.ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await db.Brands.FindAsync(id);
        }

        public async Task<bool> IsNameExistAsync(string name, int? excludeId = null)
        {
            // لو في excludeId يبقى ده للـ  [Authorize(Role.Administrator)]يعني لو الاسم موجود بس لمنتج تاني اقبله)
            // لو مش في excludeId يبقى ده للـ Create (أي تكرار مرفوض)
            return await db.Brands.AnyAsync(b => b.Name == name && (!excludeId.HasValue || b.Id != excludeId.Value));
        }

        public void Add(Brand brand)
        {
            db.Brands.Add(brand);
        }

        public void Update(Brand brand)
        {
            db.Brands.Update(brand);
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}