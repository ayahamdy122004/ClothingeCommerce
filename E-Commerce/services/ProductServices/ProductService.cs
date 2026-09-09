using AutoMapper;
using ClothingStore.Entities;
using E_Commerce.Entities.Data;
using E_Commerce.Entities.DTO.Models.Common;
using E_Commerce.Entities.DTO.Models.PRODUCTS;
using E_Commerce.Entities.DTO.Models.PRODUCTS.ProductFilterAndSearch;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model;
using E_Commerce.Repositorys.ProductRepo;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ProductService(IProductRepository repo, IMapper mapper, AppDbContext context)
        {
            _repo = repo;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<ProductResponseDTO>>> GetAll()
        {
            var products = await _repo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ProductResponseDTO>>(products);

            return new ApiResponse<IEnumerable<ProductResponseDTO>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Products retrieved successfully.",
                Data = dtos
            };
        }

        public async Task<ApiResponse<ProductResponseDTO>> AddProduct(CreateProductRequestDTO request)
        {
            if (await _repo.IsSlugExistAsync(request.Slug))
            {
                return new ApiResponse<ProductResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "This product slug already exists.",
                    Errors = new { Slug = "Slug already exists." }
                };
            }

            var product = _mapper.Map<Product>(request);
            product.IsActive = true;
            product.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(product);
            var savedProduct = await _repo.GetByIdAsync(product.Id);

            return new ApiResponse<ProductResponseDTO>
            {
                StatusCode = 201,
                Success = true,
                Message = "Product created successfully.",
                Data = _mapper.Map<ProductResponseDTO>(savedProduct)
            };
        }

        public async Task<ApiResponse<ProductResponseDTO>> UpdateProduct(int id, UPdateProductRequestDTO request)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
            {
                return new ApiResponse<ProductResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Product does not exist."
                };
            }

            _mapper.Map(request, product);
            product.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(product);
            var updatedProduct = await _repo.GetByIdAsync(id);

            return new ApiResponse<ProductResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "Product updated successfully.",
                Data = _mapper.Map<ProductResponseDTO>(updatedProduct)
            };
        }

        public async Task<ApiResponse<IEnumerable<ProductListResponseDTO>>> GetProductListForCustomerAsync()
        {
            var products = await _repo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ProductListResponseDTO>>(products);

            return new ApiResponse<IEnumerable<ProductListResponseDTO>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Customer product list retrieved successfully.",
                Data = dtos
            };
        }

        public async Task<ApiResponse<bool>> UpdateStatusAsync(int id, bool isActive)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
            {
                return new ApiResponse<bool>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Product not found.",
                    Data = false
                };
            }

            product.IsActive = isActive;
            product.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(product);

            return new ApiResponse<bool>
            {
                StatusCode = 200,
                Success = true,
                Message = "Product status updated successfully.",
                Data = true
            };
        }

        public async Task<ApiResponse<ProductDetailsResponseDTO>> GetProductDetailsByIdAsync(int id)
        {
            var productObj = await _repo.GetByIdAsync(id);
            if (productObj == null)
            {
                return new ApiResponse<ProductDetailsResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Product not found."
                };
            }

            var product = _mapper.Map<Product>(productObj);
            if (!product.IsActive)
            {
                return new ApiResponse<ProductDetailsResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Product is inactive."
                };
            }

            return new ApiResponse<ProductDetailsResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "Product details retrieved successfully.",
                Data = _mapper.Map<ProductDetailsResponseDTO>(product)
            };
        }

        public async Task<ApiResponse<ProductResponseDTO>> GetProductBySlug(string slug)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Slug == slug);
            if (product == null)
            {
                return new ApiResponse<ProductResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Product not found."
                };
            }

            return new ApiResponse<ProductResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "Product retrieved successfully.",
                Data = _mapper.Map<ProductResponseDTO>(product)
            };
        }

        public async Task<ApiResponse<PaginatedResponseDTO<ProductResponseDTO>>> GetProducts(ProductQueryDTO query)
        {
            var products = await _repo.GetAllAsync();
            var productQuery = products.AsQueryable().Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(query.Search))
                productQuery = productQuery.Where(p => p.Name.Contains(query.Search));

            if (query.CategoryId.HasValue)
                productQuery = productQuery.Where(p => p.CategoryId == query.CategoryId.Value);

            if (query.BrandId.HasValue)
                productQuery = productQuery.Where(p => p.BrandId == query.BrandId.Value);

            if (!string.IsNullOrWhiteSpace(query.Size))
                productQuery = productQuery.Where(p => p.Variations.Any(v => v.Size == query.Size && v.IsActive));

            if (!string.IsNullOrWhiteSpace(query.Color))
                productQuery = productQuery.Where(p => p.Variations.Any(v => v.Color == query.Color && v.IsActive));

            if (query.MinPrice.HasValue)
                productQuery = productQuery.Where(p => (p.DiscountPrice ?? p.BasePrice) >= query.MinPrice.Value);

            if (query.MaxPrice.HasValue)
                productQuery = productQuery.Where(p => (p.DiscountPrice ?? p.BasePrice) <= query.MaxPrice.Value);

            if (query.InStockOnly == true)
                productQuery = productQuery.Where(p => p.Variations.Any(v => v.StockQuantity > 0 && v.IsActive));

            if (query.IsFeatured.HasValue)
                productQuery = productQuery.Where(p => p.IsFeatured == query.IsFeatured.Value);

            productQuery = query.Sort?.ToLower() switch
            {
                "newest" => productQuery.OrderByDescending(p => p.CreatedAt),
                "name" => productQuery.OrderBy(p => p.Name),
                "priceasc" => productQuery.OrderBy(p => p.DiscountPrice ?? p.BasePrice),
                "pricedesc" => productQuery.OrderByDescending(p => p.DiscountPrice ?? p.BasePrice),
                _ => productQuery.OrderByDescending(p => p.CreatedAt)
            };

            var totalRecords = productQuery.Count();

            var productsAfterPagination = productQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var result = productsAfterPagination.Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                ShortDescription = p.ShortDescription,
                BrandName = p.Brand?.Name,
                CategoryName = p.Category?.Name,
                BasePrice = p.BasePrice,
                DiscountPrice = p.DiscountPrice,
                CoverImageUrl = p.CoverImageUrl,
                Material = p.Material,
                Gender = p.Gender,
                CareInstructions = p.CareInstructions,
                IsActive = p.IsActive
            }).ToList();

            var paginatedData = new PaginatedResponseDTO<ProductResponseDTO>(
                query.PageNumber,
                query.PageSize,
                totalRecords,
                result);

            return new ApiResponse<PaginatedResponseDTO<ProductResponseDTO>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Paginated products retrieved successfully.",
                Data = paginatedData
            };
        }
    }
}