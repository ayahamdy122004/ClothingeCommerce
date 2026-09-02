namespace E_Commerce.Entities.Model
{
    public class CartItem
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
