using E_Commerce.Entities.DTO.Models.PRODUCTS;

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
        
    }
}
