using AutoMapper;
using ClothingStore.Entities;
using E_Commerce.Entities.DTO.Models.PRODUCTS;
using E_Commerce.Entities.Model;
using E_Commerce.Repositorys.ProductRepo;

namespace E_Commerce.services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repo;
        private readonly IMapper mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<ProductResponseDTO> AddProduct(CreateProductRequestDTO request)
        {
            // 1. Business Validation
            if (await repo.IsSlugExistAsync(request.Slug))
                throw new Exception("This product slug already exists.");

            // 2. إنشاء الـ Entity باستخدام AutoMapper
            var product = mapper.Map<Product>(request);
            product.IsActive = true;
            product.CreatedAt = DateTime.UtcNow;

            // 3. حفظ المنتج
            await repo.AddAsync(product);

            // 4. جلب المنتج تاني
            var savedProduct = await repo.GetByIdAsync(product.Id);

            // 5. الـ Mapping للـ Response
            return mapper.Map<ProductResponseDTO>(savedProduct);
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAll()
        {
            var products = await repo.GetAllAsync();
            return mapper.Map<IEnumerable<ProductResponseDTO>>(products);
        }

        public async Task<ProductResponseDTO> UpdateProduct(int id, UPdateProductRequestDTO pro)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product does not exist.");

            // تحديث بيانات الكائن الموجود مباشرة باستخدام AutoMapper
            mapper.Map(pro, product);
            product.UpdatedAt = DateTime.UtcNow;

            await repo.UpdateAsync(product);

            var updatedProduct = await repo.GetByIdAsync(id);
            return mapper.Map<ProductResponseDTO>(updatedProduct);
        }

        public async Task<IEnumerable<ProductListResponseDTO>> GetProductListForCustomerAsync()
        {
            var products = await repo.GetAllAsync();
            return mapper.Map<IEnumerable<ProductListResponseDTO>>(products);
        }

        public async Task<bool> UpdateStatusAsync(int id, bool isActive)
        {
            var p = await repo.GetByIdAsync(id);
            if (p == null) return false;

            p.IsActive = isActive;
            p.UpdatedAt = DateTime.UtcNow;

            await repo.UpdateAsync(p);
            return true;
        }
        public async Task<ProductDetailsResponseDTO?> GetProductDetailsByIdAsync(int id)
        {
            // 1. جلب الكائن من الـ Repo
            var productObj = await repo.GetByIdAsync(id);

            if (productObj == null)
                return null;

            // 2. Cast صريح للـ Product حتى تفهم AutoMapper النوع الأصلي
            var product = productObj as Product;

            if (product == null || !product.IsActive)
                return null;

            // 3. التحويل بـ AutoMapper
            return mapper.Map<ProductDetailsResponseDTO>(product);
        }
    }
}