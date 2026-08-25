namespace E_Commerce.Entities.DTO.Models.Variation
{
    public class VariationProductResponseDTO
    {
        public int Id { get; set; }

        // مفيد لو الفرونت عايز يعرف التنويعة دي تبع منتج إيه
        public int ProductId { get; set; }

        public string Color { get; set; }
        public string Size { get; set; }
        public string SKU { get; set; }
        public int StockQuantity { get; set; }
        public decimal? PriceAdjustment { get; set; }
        public bool IsActive { get; set; }
    }
}