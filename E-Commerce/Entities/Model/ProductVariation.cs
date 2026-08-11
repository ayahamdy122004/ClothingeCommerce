using ClothingStore.Entities;

namespace E_Commerce.Entities.Model
{
    public class ProductVariation
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string SKU { get; set; }

        public int StockQuantity { get; set; }

        public decimal? PriceAdjustment { get; set; }

        public bool IsActive { get; set; }


        // Navigation Properties

        public Product Product { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}