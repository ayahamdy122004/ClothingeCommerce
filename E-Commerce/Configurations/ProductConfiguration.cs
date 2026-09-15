using ClothingStore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // 1. تحديد الـ Primary Key
            builder.HasKey(p => p.Id);

            // 2. إعداد الـ Unique Index للـ Slug
            builder.HasIndex(p => p.Slug)
                   .IsUnique();
          
            // 3. علاقة الـ Brand مع الـ Product (Restrict Delete)
            builder.HasOne(p => p.Brand)
                   .WithMany(b => b.Products)
                   .HasForeignKey(p => p.BrandId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 4. علاقة الـ Category مع الـ Product (Restrict Delete)
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 5. قيود إضافية تحسّن قاعدة البيانات (Optional but recommended)
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Slug)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.BasePrice)
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.DiscountPrice)
                   .HasColumnType("decimal(18,2)");
        }
    }
}