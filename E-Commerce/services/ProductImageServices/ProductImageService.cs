using AutoMapper;
using E_Commerce.Entities.DTO.Models.PRODUCTIMAGEFolder;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model;
using E_Commerce.Repositorys.ProductImageRepo;
using E_Commerce.Repositorys.ProductRepo;
using Microsoft.AspNetCore.Hosting;

namespace E_Commerce.services.ProductServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _imageRepo;
        private readonly IProductRepository _productRepo;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper mapper;
        public ProductImageService(
            IProductImageRepository imageRepo,
            IProductRepository productRepo,
            IWebHostEnvironment environment, IMapper mapper)
        {
            _imageRepo = imageRepo;
            _productRepo = productRepo;
            _environment = environment;
            this.mapper = mapper;
        }

        // 1. Upload Image
        public async Task<ApiResponse<ProductImageResponseDTO>> UploadImageAsync(UploadImageDTO dto)
        {
            var productExists = await _productRepo.GetByIdAsync(dto.ProductId);
            if (productExists == null)
            {
                return ApiResponse<ProductImageResponseDTO>.FailureResponse("Product not found.", 404);
            }

            if (dto.File == null || dto.File.Length == 0)
            {
                return ApiResponse<ProductImageResponseDTO>.FailureResponse("Please provide a valid image file.", 400);
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "products");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(fileStream);
            }

            var imageRelativePath = $"/uploads/products/{uniqueFileName}";

            if (dto.IsCover)
            {
                await _imageRepo.ResetCoverImagesAsync(dto.ProductId);
            }

            var productImage = new ProductImage
            {
                ProductId = dto.ProductId,
                ImageUrl = imageRelativePath,
                AlternativeText = dto.AlternativeText,
                IsCover = dto.IsCover,
                DisplayOrder = 0
            };

            var savedImage = await _imageRepo.UploadImageAsync(productImage);

            var responseDto = new ProductImageResponseDTO
            {
                Id = savedImage.Id,
                ProductId = savedImage.ProductId,
                ImageUrl = savedImage.ImageUrl,
                AlternativeText = savedImage.AlternativeText,
                IsCover = savedImage.IsCover,
                DisplayOrder = savedImage.DisplayOrder
            };

            return ApiResponse<ProductImageResponseDTO>.SuccessResponse(responseDto, "Image uploaded successfully.", 201);
        }

        // 2. Change Display Order
        public async Task<ApiResponse<bool>> ChangeDisplayOrderAsync(ChangeOrderDTo dto)
        {
            var image = await _imageRepo.GetByIdAsync(dto.ImgId);
            if (image == null)
            {
                return ApiResponse<bool>.FailureResponse("Image not found.", 404);
            }

            image.DisplayOrder = dto.OrderDisplay;
            await _imageRepo.UpdateImageAsync(image);

            return ApiResponse<bool>.SuccessResponse(true, "Image display order updated successfully.", 200);
        }

        // 3. Select Cover Image
        public async Task<ApiResponse<bool>> SelectCoverImageAsync(SelectCoverDTO dto)
        {
            var image = await _imageRepo.GetByIdAsync(dto.ImgId);
            if (image == null)
            {
                return ApiResponse<bool>.FailureResponse("Image not found.", 404);
            }

            await _imageRepo.ResetCoverImagesAsync(image.ProductId);

            image.IsCover = true;
            await _imageRepo.UpdateImageAsync(image);

            return ApiResponse<bool>.SuccessResponse(true, "Cover image set successfully.", 200);
        }

        // 4. Delete Image
        public async Task<ApiResponse<bool>> DeleteImageAsync(int imgId)
        {
            var image = await _imageRepo.GetByIdAsync(imgId);
            if (image == null)
            {
                return ApiResponse<bool>.FailureResponse("Image not found.", 404);
            }

            if (!string.IsNullOrEmpty(image.ImageUrl))
            {
                var physicalPath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                }
            }

            await _imageRepo.DeleteImageAsync(image);

            return ApiResponse<bool>.SuccessResponse(true, "Image deleted successfully.", 200);
        }

        // 5. Get All Images for Product
        public async Task<ApiResponse<IEnumerable<ProductImageResponseDTO>>> GetImagesByProductIdAsync(int productId)
        {
            var images = await _imageRepo.GetByProductIdAsync(productId);
            if(images == null)
                return ApiResponse<IEnumerable<ProductImageResponseDTO>>.FailureResponse("No images found for the specified product.", 404);

            var responseDtos = images.Select(img => new ProductImageResponseDTO
            {
                Id = img.Id,
                ProductId = img.ProductId,
                ImageUrl = img.ImageUrl,
                AlternativeText = img.AlternativeText,
                IsCover = img.IsCover,
                DisplayOrder = img.DisplayOrder
            });

            return ApiResponse<IEnumerable<ProductImageResponseDTO>>.SuccessResponse(responseDtos, "Images retrieved successfully.", 200);
        }

        public async Task<ApiResponse<ProductImageResponseDTO>> UpdateImage(int ImgId)
        {
           var a= await _imageRepo.GetByIdAsync(ImgId);
            if(a == null)
            {
                return ApiResponse<ProductImageResponseDTO>.FailureResponse("Image not found.", 404);
            }
            a.IsCover = true;
            var x=mapper.Map<ProductImageResponseDTO>(a);
            return ApiResponse<ProductImageResponseDTO>.SuccessResponse(x, "Image deleted successfully.", 200);
        }
    }
}