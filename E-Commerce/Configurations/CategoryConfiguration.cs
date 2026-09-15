using ClothingStore.Entities;
using E_Commerce.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Name).IsUnique();

            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        }
    }
}