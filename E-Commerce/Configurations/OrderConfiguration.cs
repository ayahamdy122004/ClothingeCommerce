using ClothingStore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.HasIndex(o => o.OrderNumber).IsUnique();

            // Required shipping-address fields
            builder.Property(o => o.Street).IsRequired().HasMaxLength(200);
            builder.Property(o => o.City).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Country).IsRequired().HasMaxLength(100);
            builder.Property(o => o.RecipientName).IsRequired().HasMaxLength(100);
            builder.Property(o => o.DeliveryPhoneNumber).IsRequired().HasMaxLength(20);

            // Totals
            builder.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
            builder.Property(o => o.ShippingCost).HasColumnType("decimal(18,2)");
            builder.Property(o => o.FinalTotal).HasColumnType("decimal(18,2)");

            // Valid statuses saved as strings or enums
            builder.Property(o => o.OrderStatus).IsRequired().HasMaxLength(50);
            builder.Property(o => o.ShipmentStatus).IsRequired().HasMaxLength(50);
            builder.Property(o => o.PaymentStatus).IsRequired().HasMaxLength(50);

            builder.HasMany(o => o.OrderItems)
                   .WithOne()
                   .HasForeignKey(i => i.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}