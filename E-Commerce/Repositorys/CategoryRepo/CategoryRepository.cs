using E_Commerce.Entities;
using E_Commerce.Entities.Data;
using E_Commerce.Entities.Model;
using E_Commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> IsNameExistAsync(string name, int? excludeId = null)
        {
            return await _context.Categories.
                AnyAsync(b => b.Name == name && (!excludeId.HasValue || b.Id != excludeId.Value));
        }


        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        Task<IEnumerable<Category>> ICategoryRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

    }
}