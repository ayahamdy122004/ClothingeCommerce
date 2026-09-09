using E_Commerce.Entities.DTO.Models.Common;
using E_Commerce.Entities.DTO.Models.PRODUCTS;
using E_Commerce.Entities.DTO.Models.PRODUCTS.ProductFilterAndSearch;
using E_Commerce.Entities.DTO.ResponseAPIs;

namespace E_Commerce.services.ProductServices
{
    public interface IProductService
    {
        Task<ApiResponse<IEnumerable<ProductResponseDTO>>> GetAll();
        Task<ApiResponse<IEnumerable<ProductListResponseDTO>>> GetProductListForCustomerAsync();
        Task<ApiResponse<ProductResponseDTO>> AddProduct(CreateProductRequestDTO pro);
        Task<ApiResponse<ProductResponseDTO>> UpdateProduct(int id, UPdateProductRequestDTO pro);
        Task<ApiResponse<ProductDetailsResponseDTO>> GetProductDetailsByIdAsync(int id);
        Task<ApiResponse<ProductResponseDTO>> GetProductBySlug(string slug);
        Task<ApiResponse<bool>> UpdateStatusAsync(int id, bool isActive);
        Task<ApiResponse<PaginatedResponseDTO<ProductResponseDTO>>> GetProducts(ProductQueryDTO query);
    }
}