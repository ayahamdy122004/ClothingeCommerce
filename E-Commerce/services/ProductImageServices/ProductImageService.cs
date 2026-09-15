using AutoMapper;
using E_Commerce.Entities.DTO.Models.ProductImages;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model;
using E_Commerce.Repositorys.ProductImageRepo;
using E_Commerce.Repositorys.ProductRepo;

namespace E_Commerce.services.ProductServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _imageRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public ProductImageService(
            IProductImageRepository imageRepo,
            IProductRepository productRepo,
            IMapper mapper)
        {
            _imageRepo = imageRepo;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<ProductImageResponseDTO>>> UploadImagesAsync(UploadImageRequestDTO request)
        {
            var product = await _productRepo.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return new ApiResponse<IEnumerable<ProductImageResponseDTO>>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Product not found."
                };
            }

            if (request.Images == null || !request.Images.Any())
            {
                return new ApiResponse<IEnumerable<ProductImageResponseDTO>>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "No images provided for upload."
                };
            }

            var uploadedImages = new List<ProductImage>();

            foreach (var item in request.Images)
            {
                if (item.File != null && item.File.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{item.File.FileName}";
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var filePath = Path.Combine(folderPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.File.CopyToAsync(stream);
                    }

                    uploadedImages.Add(new ProductImage
                    {
                        ProductId = request.ProductId,
                        ImageUrl = $"/images/products/{fileName}",
                        AlternativeText = item.AlternativeText ?? product.Name,
                        DisplayOrder = item.DisplayOrder,
                        IsCover = item.IsCover
                    });
                }
            }

            if (uploadedImages.Any(img => img.IsCover))
            {
                await _imageRepo.ResetCoverImagesAsync(request.ProductId);
            }

            await _imageRepo.AddRangeAsync(uploadedImages);

            var mappedResult = _mapper.Map<IEnumerable<ProductImageResponseDTO>>(uploadedImages);

            return new ApiResponse<IEnumerable<ProductImageResponseDTO>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Images uploaded successfully.",
                Data = mappedResult
            };
        }

        public async Task<ApiResponse<IEnumerable<ProductImageResponseDTO>>> GetImagesByProductIdAsync(int productId)
        {
            var images = await _imageRepo.GetByProductIdAsync(productId);
            var mappedResult = _mapper.Map<IEnumerable<ProductImageResponseDTO>>(images);

            return new ApiResponse<IEnumerable<ProductImageResponseDTO>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Images retrieved successfully.",
                Data = mappedResult
            };
        }

        public async Task<ApiResponse<bool>> DeleteImageAsync(int imageId)
        {
            var image = await _imageRepo.GetByIdAsync(imageId);
            if (image == null)
            {
                return new ApiResponse<bool>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Image not found.",
                    Data = false
                };
            }

            var relativePath = image.ImageUrl.TrimStart('/');
            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }

            await _imageRepo.DeleteAsync(image);

            return new ApiResponse<bool>
            {
                StatusCode = 200,
                Success = true,
                Message = "Image deleted successfully.",
                Data = true
            };
        }
    }
}