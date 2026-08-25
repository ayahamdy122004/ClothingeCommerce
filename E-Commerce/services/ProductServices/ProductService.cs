using ClothingStore.Entities;
using E_Commerce.Entities;
using E_Commerce.Entities.DTO.Models.PRODUCTS;

using E_Commerce.Repositorys.ProductRepo;
namespace E_Commerce.services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repo;

        public ProductService(IProductRepository repo)
        {
            this.repo = repo;
        }
     

        public async Task<ProductResponseDTO> AddProduct(CreateProductRequestDTO request)
        {
            // 1. Business Validation
            if (await repo.IsSlugExistAsync(request.Slug))
                throw new Exception("This product slug already exists.");

            // 2. إنشاء الـ Entity
            var product = new Product
            {
                Name = request.Name,
                Slug = request.Slug,
                ShortDescription = request.ShortDescription,
                FullDescription = request.FullDescription,
                BrandId = request.BrandId,
                CategoryId = request.CategoryId,
                BasePrice = request.BasePrice,
                CoverImageUrl = request.CoverImageUrl,
                Material = request.Material,
                Gender = request.Gender,
                CareInstructions = request.CareInstructions,
                IsFeatured = request.IsFeatured,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // 3. حفظ المنتج
            await repo.AddAsync(product);

            // 4. جلب المنتج تاني (عشان الـ Names)
            var savedProduct = await repo.GetByIdAsync(product.Id);

            // 5. الـ Mapping
            return new ProductResponseDTO
            {
                Id = savedProduct.Id,
                Name = savedProduct.Name,
                Slug = savedProduct.Slug,
                ShortDescription = savedProduct.ShortDescription,
                BrandName = savedProduct.Brand.Name,
                CategoryName = savedProduct.Category.Name,
                BasePrice = savedProduct.BasePrice,
                DiscountPrice = savedProduct.DiscountPrice,
                CoverImageUrl = savedProduct.CoverImageUrl,
                Material = savedProduct.Material,
                Gender = savedProduct.Gender,
                CareInstructions = savedProduct.CareInstructions,
                IsActive = savedProduct.IsActive
            };
        }

  
        public async Task<IEnumerable<ProductResponseDTO>> GetAll()
        {
            var products = await repo.GetAllAsync(); // محتاجة تكون في الـ Repo فيها Include

            return products.Select(c => new ProductResponseDTO
            {
                Id = c.Id,
                Name = c.Name,
                ShortDescription = c.ShortDescription,
                BasePrice = c.BasePrice,
                BrandName = c.Brand != null ? c.Brand.Name : "Unknown", // احتياطي لو مفيش Include
                CategoryName = c.Category != null ? c.Category.Name : "Unknown",
                Slug = c.Slug,
                DiscountPrice = c.DiscountPrice,
                CoverImageUrl = c.CoverImageUrl,
                Material = c.Material,
                Gender = c.Gender,
                CareInstructions = c.CareInstructions,
                IsActive = c.IsActive
            }).ToList();
        }
     
        public async Task<ProductResponseDTO> UpdateProduct(int id, UPdateProductRequestDTO pro)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product does not exist.");

            // تحديث البيانات
            product.Name = pro.Name;
            product.Slug = pro.Slug;
            product.ShortDescription = pro.ShortDescription;
            product.FullDescription = pro.FullDescription;
            product.BasePrice = pro.BasePrice;
            product.DiscountPrice = pro.DiscountPrice;
            product.CoverImageUrl = pro.CoverImageUrl;
            product.Gender = pro.Gender;
            product.CareInstructions = pro.CareInstructions;
            product.IsActive = pro.IsActive;
            product.IsFeatured = pro.IsFeatured;

            // ⚠️ الصح: نحدث الـ ID مش اسم البراند!
            product.BrandId = pro.BrandId;
            product.CategoryId = pro.CategoryId;

            product.UpdatedAt = DateTime.UtcNow; // تحديث وقت آخر تعديل

            // حفظ التعديلات في الداتا بيز
            await repo.UpdateAsync(product);

            // جلب المنتج تاني عشان نرجع الـ Response بتاعه بالأسماء الجديدة
            var updatedProduct = await repo.GetByIdAsync(id);

            return new ProductResponseDTO
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Slug = updatedProduct.Slug,
                ShortDescription = updatedProduct.ShortDescription,
                BrandName = updatedProduct.Brand.Name,
                CategoryName = updatedProduct.Category.Name,
                BasePrice = updatedProduct.BasePrice,
                DiscountPrice = updatedProduct.DiscountPrice,
                CoverImageUrl = updatedProduct.CoverImageUrl,
                Material = updatedProduct.Material,
                Gender = updatedProduct.Gender,
                CareInstructions = updatedProduct.CareInstructions,
                IsActive = updatedProduct.IsActive
            };
        }
   
        public async Task<bool> UpdateStatusAsync(int id, bool isActive)
        {
            var p = await repo.GetByIdAsync(id);
            if (p == null) return false;

            p.IsActive = isActive;
            p.UpdatedAt = DateTime.UtcNow;

            // لو الـ UpdateAsync بتاعتك بتعمل SaveChanges جواها، مش محتاجة تكراريها
            await repo.UpdateAsync(p);

            return true;
        }
    }
}