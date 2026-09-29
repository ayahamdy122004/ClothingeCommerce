using ClothingStore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.ProductName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(oi => oi.UnitPrice)
                   .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.LineTotal)
                   .HasColumnType("decimal(18,2)");
        }
    }
}