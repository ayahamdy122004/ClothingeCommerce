using AutoMapper;
using E_Commerce.Entities.DTO.Models.ProductImages;
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

        public async Task<IEnumerable<ProductImageUploadItemDTO>> UploadImagesAsync(UploadImageRequestDTO request)
        {
            var product = await _productRepo.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new Exception("Product not found.");

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

            return _mapper.Map<IEnumerable<ProductImageUploadItemDTO>>(uploadedImages);
        }






        // 2. جلب كافة صور منتج معين مرتبة
        public async Task<IEnumerable<ProductImageUploadItemDTO>> GetImagesByProductIdAsync(int productId)
        {
            var images = await _imageRepo.GetByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<ProductImageUploadItemDTO>>(images);
        }

        // 3. حذف صورة من الهارد والداتابيز
        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var image = await _imageRepo.GetByIdAsync(imageId);
            if (image == null) return false;

            var relativePath = image.ImageUrl.TrimStart('/');
            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }

            await _imageRepo.DeleteAsync(image);

            return true;
        }
    }
}