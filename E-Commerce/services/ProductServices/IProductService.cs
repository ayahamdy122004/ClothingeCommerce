using E_Commerce.Entities.DTO.Models.PRODUCTS;

namespace E_Commerce.services.ProductServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAll();
        Task<ProductResponse> UpdateProduct(int id,UpdateProductRequest pro);
        Task<ProductResponse> AddProduct(CreateProductRequest pro);
   
        Task<bool> UpdateStatusAsync(int id, bool isActive);
        
    }
}
