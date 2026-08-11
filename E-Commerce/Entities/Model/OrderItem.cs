using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }

        public int ProductId { get; set; }

        public int? ProductVariationId { get; set; } // Nullable لو منتج مش بيكVariations

        // ====== Snapshot Data (بيانات تتسجل وقت الطلب وما تتتغيرش) ======
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? SKU { get; set; }

        [MaxLength(500)]
        public string? CoverImageUrl { get; set; }

        [MaxLength(50)]
        public string? SelectedColor { get; set; }

        [MaxLength(50)]
        public string? SelectedSize { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }
    }
}