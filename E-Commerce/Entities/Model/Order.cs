
using E_Commerce.Entities.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty; // مثال: ORD-2026-000105

        public string CustomerId { get; set; } = string.Empty;

        [ForeignKey(nameof(CustomerId))]
        public ApplicationUser? Customer { get; set; }

        // Snapshot لبيانات العميل وقت الطلب
        [MaxLength(100)]
        public string CustomerFirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string CustomerLastName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string CustomerEmail { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? CustomerPhoneNumber { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // الحالات (Statuses)
        [MaxLength(50)]
        public string OrderStatus { get; set; } = "Pending";

        [MaxLength(50)]
        public string ShipmentStatus { get; set; } = "NotPrepared";

        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "CashOnDelivery";

        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Unpaid";

        // المبالغ المالية
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalTotal { get; set; }

        public string? CustomerNotes { get; set; }

        // ============ عنوان الشحن (Shipping Address) ============
        [MaxLength(100)]
        public string RecipientName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string DeliveryPhoneNumber { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Governorate { get; set; }

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Street { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? BuildingNumber { get; set; }

        [MaxLength(10)]
        public string? Floor { get; set; }

        [MaxLength(10)]
        public string? ApartmentNumber { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(200)]
        public string? Landmark { get; set; }

        public string? DeliveryNotes { get; set; }

        // ============ بيانات الشحن والتتبع ============
        [MaxLength(100)]
        public string? DeliveryCompanyName { get; set; }

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation Property لربطه بعناصر الطلب
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}