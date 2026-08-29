using E_Commerce.Entities.DTO.Models.Common;
using E_Commerce.Entities.DTO.Models.PRODUCTS;
using E_Commerce.Entities.DTO.Models.PRODUCTS.ProductFilterAndSearch;

namespace E_Commerce.services.ProductServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAll();
        Task<IEnumerable<ProductListResponseDTO>> GetProductListForCustomerAsync();
        Task<ProductResponseDTO> UpdateProduct(int id,UPdateProductRequestDTO pro);
        Task<ProductResponseDTO> AddProduct(CreateProductRequestDTO pro);
        Task<ProductDetailsResponseDTO?> GetProductDetailsByIdAsync(int id);
        Task<bool> UpdateStatusAsync(int id, bool isActive);
        // product  by slug 
        Task<ProductResponseDTO> GetProductBySlug(string slug); 
        // Module 7
        Task<PaginatedResponseDTO<ProductResponseDTO>> GetProducts(
            ProductQueryDTO query);


    }
}
