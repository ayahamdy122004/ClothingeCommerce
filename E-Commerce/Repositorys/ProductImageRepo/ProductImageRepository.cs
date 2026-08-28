using E_Commerce.Entities.Data; // أو الـ DbContext بتاعك
using E_Commerce.Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositorys.ProductImageRepo
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext db;

        public ProductImageRepository(AppDbContext db)
        {
            this.db = db;
        }

        public async Task AddRangeAsync(IEnumerable<ProductImage> images)
        {
            await db.ProductImages.AddRangeAsync(images);
            await db.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId)
        {
            return await db.ProductImages
                .Where(img => img.ProductId == productId)
                .OrderBy(img => img.DisplayOrder) // ترتيب الصور بحسب DisplayOrder
                .ToListAsync();
        }
        public async Task ResetCoverImagesAsync(int productId)
        {
            var existingCovers = await db.ProductImages
                .Where(img => img.ProductId == productId && img.IsCover)
                .ToListAsync();

            foreach (var img in existingCovers)
            {
                img.IsCover = false;
            }
        }

        public async Task<ProductImage?> GetByIdAsync(int id)
        {
            return await db.ProductImages.FindAsync(id);
        }

        public async Task DeleteAsync(ProductImage image)
        {
            db.ProductImages.Remove(image);
            await db.SaveChangesAsync();
        }
    }
}