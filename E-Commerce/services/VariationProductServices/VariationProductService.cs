using E_Commerce.Entities.DTO.Models.Variation;
using E_Commerce.Entities.Model;
using E_Commerce.Repositorys.VariationRepo;

namespace E_Commerce.services.VariationProductServices
{
    public class VariationProductService : IVariationProductService
    {
        private readonly IVariationRepository variationRepository;

        public VariationProductService(
            IVariationRepository variationRepository)
        {
            this.variationRepository = variationRepository;
        }
        public async Task<VariationProductResponse> Create(int productId,CreateVariationProduct variationProduct)
        {
            var isSkuExist = await variationRepository
                .IsSkuExistAsync(variationProduct.SKU);

            if (isSkuExist)
            {
                throw new Exception("SKU already exists");
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

            var result = await variationRepository.Add(variation);

            return new VariationProductResponse
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
        }

        // =============================
        // Update Variation
        // =============================
        public async Task<VariationProductResponse> Update(
            int id,
            UpdateVariationProduct variationProduct)
        {
            var variation = await variationRepository.GetById(id);

            if (variation == null)
            {
                throw new Exception("Variation not found");
            }

            var isSkuExist = await variationRepository
                .IsSkuExistAsync(variationProduct.SKU, id);

            if (isSkuExist)
            {
                throw new Exception("SKU already exists");
            }

            variation.Color = variationProduct.Color;
            variation.Size = variationProduct.Size;
            variation.SKU = variationProduct.SKU;
            variation.StockQuantity = variationProduct.StockQuantity;
            variation.PriceAdjustment = variationProduct.PriceAdjustment;
            variation.IsActive = variationProduct.IsActive;

            var result = await variationRepository.Update(variation);

            return new VariationProductResponse
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
        }


        // =============================
        // Get All
        // =============================
        public async Task<IEnumerable<VariationProductResponse>> GetAll()
        {
            var variations = await variationRepository.GetAll();

            return variations.Select(v => new VariationProductResponse
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
        }


        // =============================
        // Get By Id
        // =============================
        public async Task<VariationProductResponse> GetById(int id)
        {
            var variation = await variationRepository.GetById(id);

            if (variation == null)
            {
                throw new Exception("Variation not found");
            }

            return new VariationProductResponse
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
        }


        // =============================
        // Check SKU
        // =============================
        public async Task<bool> IsSkuExistAsync(
            string sku,
            int? excludeId = null)
        {
            return await variationRepository
                .IsSkuExistAsync(sku, excludeId);
        }
    }
}