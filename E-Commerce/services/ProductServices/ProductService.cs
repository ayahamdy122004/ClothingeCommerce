using AutoMapper;
using ClothingStore.Entities;
using E_Commerce.Entities.Data;
using E_Commerce.Entities.DTO.Models.Common;
using E_Commerce.Entities.DTO.Models.PRODUCTS;
using E_Commerce.Entities.DTO.Models.PRODUCTS.ProductFilterAndSearch;
using E_Commerce.Entities.Model;
using E_Commerce.Repositorys.ProductRepo;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repo;
        private readonly IMapper mapper;
        private readonly AppDbContext context;
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

            // نحول الكائن لـ Product بـ AutoMapper بدل الكاست اليدوي
            var product = mapper.Map<Product>(productObj);

            if (!product.IsActive)
                return null;

            return mapper.Map<ProductDetailsResponseDTO>(product);

        }
        /// <summary>
        /// /////////////////////
        /// </summary>
        /// <param name="specParams"></param>
        /// <returns></returns>
        public async Task<PaginatedResponseDTO<ProductResponseDTO>> GetProducts(
      ProductQueryDTO query)
        {
            var products = await repo.GetAllAsync();

            // نحولها Query عشان نقدر نعمل عليها
            // Search + Filter + Sort
            var productQuery = products.AsQueryable();


            // =========================
            // 1. عرض المنتجات الـ Active فقط
            // =========================

            productQuery = productQuery.Where(p => p.IsActive);


            // =========================
            // 2. Search by Product Name
            // =========================

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                productQuery = productQuery.Where(p =>
                    p.Name.Contains(query.Search));
            }


            // =========================
            // 3. Filter by Category
            // =========================

            if (query.CategoryId.HasValue)
            {
                productQuery = productQuery.Where(p =>
                    p.CategoryId == query.CategoryId.Value);
            }


            // =========================
            // 4. Filter by Brand
            // =========================

            if (query.BrandId.HasValue)
            {
                productQuery = productQuery.Where(p =>
                    p.BrandId == query.BrandId.Value);
            }


            // =========================
            // 5. Filter by Size
            // =========================

            if (!string.IsNullOrWhiteSpace(query.Size))
            {
                productQuery = productQuery.Where(p =>
                    p.Variations.Any(v =>
                        v.Size == query.Size &&
                        v.IsActive));
            }


            // =========================
            // 6. Filter by Color
            // =========================

            if (!string.IsNullOrWhiteSpace(query.Color))
            {
                productQuery = productQuery.Where(p =>
                    p.Variations.Any(v =>
                        v.Color == query.Color &&
                        v.IsActive));
            }


            // =========================
            // 7. Filter by Minimum Price
            // =========================

            if (query.MinPrice.HasValue)
            {
                productQuery = productQuery.Where(p =>
                    (p.DiscountPrice ?? p.BasePrice) >= query.MinPrice.Value);
            }


            // =========================
            // 8. Filter by Maximum Price
            // =========================

            if (query.MaxPrice.HasValue)
            {
                productQuery = productQuery.Where(p =>
                    (p.DiscountPrice ?? p.BasePrice) <= query.MaxPrice.Value);
            }


            // =========================
            // 9. Filter by Available Stock
            // =========================

            if (query.InStockOnly == true)
            {
                productQuery = productQuery.Where(p =>
                    p.Variations.Any(v =>
                        v.StockQuantity > 0 &&
                        v.IsActive));
            }


            // =========================
            // 10. Filter by Featured Products
            // =========================

            if (query.IsFeatured.HasValue)
            {
                productQuery = productQuery.Where(p =>
                    p.IsFeatured == query.IsFeatured.Value);
            }


            // =========================
            // 11. Sorting
            // =========================

            switch (query.Sort?.ToLower())
            {
                case "newest":
                    productQuery = productQuery
                        .OrderByDescending(p => p.CreatedAt);
                    break;

                case "name":
                    productQuery = productQuery
                        .OrderBy(p => p.Name);
                    break;

                case "priceasc":
                    productQuery = productQuery
                        .OrderBy(p => p.DiscountPrice ?? p.BasePrice);
                    break;

                case "pricedesc":
                    productQuery = productQuery
                        .OrderByDescending(p => p.DiscountPrice ?? p.BasePrice);
                    break;

                default:
                    productQuery = productQuery
                        .OrderByDescending(p => p.CreatedAt);
                    break;
            }


            // =========================
            // 12. نحسب العدد قبل Pagination
            // =========================

            var totalRecords = productQuery.Count();


            // =========================
            // 13. Pagination
            // =========================

            var productsAfterPagination = productQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();


            // =========================
            // 14. Mapping
            // =========================

            var result = productsAfterPagination.Select(p =>
                new ProductResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    ShortDescription = p.ShortDescription,

                    BrandName = p.Brand.Name,
                    CategoryName = p.Category.Name,

                    BasePrice = p.BasePrice,
                    DiscountPrice = p.DiscountPrice,

                    CoverImageUrl = p.CoverImageUrl,
                    Material = p.Material,
                    Gender = p.Gender,
                    CareInstructions = p.CareInstructions,

                    IsActive = p.IsActive
                })
                .ToList();


            // =========================
            // 15. نرجع Paginated Response
            // =========================

            return new PaginatedResponseDTO<ProductResponseDTO>(
                query.PageNumber,
                query.PageSize,
                totalRecords,
                result);
        }
    }
}