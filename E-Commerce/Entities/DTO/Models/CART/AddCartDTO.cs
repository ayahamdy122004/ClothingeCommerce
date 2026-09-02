namespace E_Commerce.Entities.DTO.Models.CART
{
    public class AddCartDTO
    {
        public int ProductId { get; set; }
        public int ProductVariationId { get; set; }
        public string ProductName { get; set; } = string.Empty;
       public string CoverImageUrl { get; set; } = string.Empty;
        public string SelectedColor { get; set; } = string.Empty;
        public string SelectedSize { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
