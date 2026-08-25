using E_Commerce.Entities.Data;
using E_Commerce.Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositorys.VariationRepo
{
    public class VariationRepository: IVariationRepository  
    {
        private readonly AppDbContext db;
        public VariationRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<ProductVariation> GetById(int id)
        {
            return await db.ProductVariations.FindAsync(id);
        }
        public async Task<IEnumerable<ProductVariation>> GetAll()
        {
            return await db.ProductVariations.ToListAsync();
        }
        public async Task<ProductVariation> Add(ProductVariation variation)
        {
            await db.ProductVariations.AddAsync(variation);
            await db.SaveChangesAsync();
            return variation;
        }
        public async Task<ProductVariation> Update( ProductVariation variation)
        {
            db.ProductVariations.Update(variation);
            await db.SaveChangesAsync();
            return variation;
        }

        public async Task<bool> IsSkuExistAsync(string sku, int? excludeId = null)
        {
            return await db.ProductVariations.AnyAsync(v => v.SKU == sku && (!excludeId.HasValue || v.Id != excludeId.Value));
        }
    }
}
