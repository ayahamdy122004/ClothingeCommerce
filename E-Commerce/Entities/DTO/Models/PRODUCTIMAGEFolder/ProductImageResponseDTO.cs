using ClothingStore.Entities;

namespace E_Commerce.Entities.DTO.Models.PRODUCTIMAGEFolder
{
    public class ProductImageResponseDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string? ImageUrl { get; set; }

        public string? AlternativeText { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsCover { get; set; }


    }
}
