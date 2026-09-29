using AutoMapper;
using E_Commerce.Entities.DTO.Models.Variation;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model;
using E_Commerce.Repositorys.ProductRepo;
using E_Commerce.Repositorys.VariationRepo;

namespace E_Commerce.services.VariationProductServices
{
    public class VariationProductService : IVariationProductService
    {
        private readonly IVariationRepository _variationRepository;
        private readonly IProductRepository Pro;
        private readonly IMapper mapper;
        public VariationProductService(IVariationRepository variationRepository, IMapper mapper, IProductRepository Pro)
        {
            _variationRepository = variationRepository;
            this.Pro = Pro;
            this.mapper = mapper;   
        }
        public async Task<ApiResponse<VariationProductResponseDTO>> Create(int productId, CreateVariationProductDTO variationProduct)
        {         var isSkuExist = await _variationRepository.IsSkuExistAsync(variationProduct.SKU);
            if (isSkuExist)
            {
                return ApiResponse<VariationProductResponseDTO>.FailureResponse("SKU already exists.", 400);
            }
            var product = await Pro.GetByIdAsync(productId);
            if(product == null)
            {
                return ApiResponse<VariationProductResponseDTO>.FailureResponse("Product not found.", 404);
            }

            var variation = mapper.Map<ProductVariation>(variationProduct);
            variation.ProductId = productId; 
            var result = await _variationRepository.Add(variation);
            var responseDto = mapper.Map<VariationProductResponseDTO>(result);
            return ApiResponse<VariationProductResponseDTO>.SuccessResponse(responseDto, "Variation created successfully.", 201);
        }

        public async Task<ApiResponse<VariationProductResponseDTO>> Update(int id, UpdateVariationProductDTO variationProduct)
        {
            var variation = await _variationRepository.GetById(id);
            if (variation == null)
            {
                return ApiResponse<VariationProductResponseDTO>.FailureResponse("Variation not found.", 404);
            }
            var isSkuExist = await _variationRepository.IsSkuExistAsync(variationProduct.SKU, id);
            if (isSkuExist)
            {
                return ApiResponse<VariationProductResponseDTO>.FailureResponse("SKU already exists for another variation.", 400);
            }
            mapper.Map(variationProduct, variation);
            var result = await _variationRepository.Update(variation);
            var dto = mapper.Map<VariationProductResponseDTO>(result);
            return ApiResponse<VariationProductResponseDTO>.SuccessResponse(dto, "Variation updated successfully.", 200);
        }
        public async Task<ApiResponse<IEnumerable<VariationProductResponseDTO>>> GetAll()
        {
            var variations = await _variationRepository.GetAll();
            var dtos = mapper.Map<IEnumerable<VariationProductResponseDTO>>(variations);
            return new ApiResponse<IEnumerable<VariationProductResponseDTO>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Variations retrieved successfully.",
                Data = dtos
            };
        }
        public async Task<ApiResponse<VariationProductResponseDTO>> GetById(int id)
        {
            var variation = await _variationRepository.GetById(id);
            if (variation == null)
            {
                return new ApiResponse<VariationProductResponseDTO>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Variation not found."
                };
            }

         var dto = mapper.Map<VariationProductResponseDTO>(variation);  

            return new ApiResponse<VariationProductResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "Variation retrieved successfully.",
                Data = dto
            };
        }

        public async Task<ApiResponse<bool>> IsSkuExistAsync(string sku, int? excludeId = null)
        {
            var exists = await _variationRepository.IsSkuExistAsync(sku, excludeId);
         

            return new ApiResponse<bool>
            {
                StatusCode = 200,
                Success = exists,
                Message = exists ? "SKU exists." : "SKU is available.",
                Data = exists
            };
        }
    }
}