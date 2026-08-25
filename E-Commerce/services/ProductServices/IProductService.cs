using E_Commerce.Entities.DTO.Models.PRODUCTS;

namespace E_Commerce.services.ProductServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAll();
        Task<ProductResponseDTO> UpdateProduct(int id,UPdateProductRequestDTO pro);
        Task<ProductResponseDTO> AddProduct(CreateProductRequestDTO pro);
   
        Task<bool> UpdateStatusAsync(int id, bool isActive);
        
    }
}
