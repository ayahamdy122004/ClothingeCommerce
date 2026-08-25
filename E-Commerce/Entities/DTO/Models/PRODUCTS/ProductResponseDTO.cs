namespace E_Commerce.Entities.DTO.Models.PRODUCTS
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }

        // هنا بنرجع الأسماء للفرونت
        public string BrandName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Material { get; set; }
        public string? Gender { get; set; }
        public string? CareInstructions { get; set; }
        public bool IsActive { get; set; }
    }
}