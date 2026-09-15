using E_Commerce.Entities.DTO.Models.Variation;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model;
using E_Commerce.Repositorys.VariationRepo;

namespace E_Commerce.services.VariationProductServices
{
    public class VariationProductService : IVariationProductService
    {
        private readonly IVariationRepository _variationRepository;

        public VariationProductService(IVariationRepository variationRepository)
        {
            _variationRepository = variationRepository;
        }

        public async Task<ApiResponse<VariationProductResponseDTO>> Create(int productId, CreateVariationProductDTO variationProduct)
        {
            var isSkuExist = await _variationRepository.IsSkuExistAsync(variationProduct.SKU);
            if (isSkuExist)
            {
                return new ApiResponse<VariationProductResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "SKU already exists."
                };
            }

            var variation = new ProductVariation
            {
                ProductId = productId,
                Color = variationProduct.Color,
                Size = variationProduct.Size,
                SKU = variationProduct.SKU,
                StockQuantity = variationProduct.StockQuantity,
                PriceAdjustment = variationProduct.PriceAdjustment,
                IsActive = variationProduct.IsActive
            };

            var result = await _variationRepository.Add(variation);

            var dto = new VariationProductResponseDTO
            {
                Id = result.Id,
                ProductId = result.ProductId,
                Color = result.Color,
                Size = result.Size,
                SKU = result.SKU,
                StockQuantity = result.StockQuantity,
                PriceAdjustment = result.PriceAdjustment,
                IsActive = result.IsActive
            };

            return new ApiResponse<VariationProductResponseDTO>
            {
                StatusCode = 201,
                Success = true,
                Message = "Variation created successfully.",
                Data = dto
            };
        }

        public async Task<ApiResponse<VariationProductResponseDTO>> Update(int id, UpdateVariationProductDTO variationProduct)
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

            var isSkuExist = await _variationRepository.IsSkuExistAsync(variationProduct.SKU, id);
            if (isSkuExist)
            {
                return new ApiResponse<VariationProductResponseDTO>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "SKU already exists for another variation."
                };
            }

            variation.Color = variationProduct.Color;
            variation.Size = variationProduct.Size;
            variation.SKU = variationProduct.SKU;
            variation.StockQuantity = variationProduct.StockQuantity;
            variation.PriceAdjustment = variationProduct.PriceAdjustment;
            variation.IsActive = variationProduct.IsActive;

            var result = await _variationRepository.Update(variation);

            var dto = new VariationProductResponseDTO
            {
                Id = result.Id,
                ProductId = result.ProductId,
                Color = result.Color,
                Size = result.Size,
                SKU = result.SKU,
                StockQuantity = result.StockQuantity,
                PriceAdjustment = result.PriceAdjustment,
                IsActive = result.IsActive
            };

            return new ApiResponse<VariationProductResponseDTO>
            {
                StatusCode = 200,
                Success = true,
                Message = "Variation updated successfully.",
                Data = dto
            };
        }

        public async Task<ApiResponse<IEnumerable<VariationProductResponseDTO>>> GetAll()
        {
            var variations = await _variationRepository.GetAll();
            var dtos = variations.Select(v => new VariationProductResponseDTO
            {
                Id = v.Id,
                ProductId = v.ProductId,
                Color = v.Color,
                Size = v.Size,
                SKU = v.SKU,
                StockQuantity = v.StockQuantity,
                PriceAdjustment = v.PriceAdjustment,
                IsActive = v.IsActive
            });

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

            var dto = new VariationProductResponseDTO
            {
                Id = variation.Id,
                ProductId = variation.ProductId,
                Color = variation.Color,
                Size = variation.Size,
                SKU = variation.SKU,
                StockQuantity = variation.StockQuantity,
                PriceAdjustment = variation.PriceAdjustment,
                IsActive = variation.IsActive
            };

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
                Success = true,
                Message = exists ? "SKU exists." : "SKU is available.",
                Data = exists
            };
        }
    }
}