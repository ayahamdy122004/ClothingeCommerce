using ClothingStore.Entities;
using E_Commerce.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(b => b.Id);

            // Unique brand names
            builder.HasIndex(b => b.Name).IsUnique();

            builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
        }
    }
}