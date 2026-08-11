using ClothingStore.Entities;
using E_Commerce.Entities.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Entities.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets لكل جدول (مفيش جدول للـ Cart أو Address زي ما اتكلمنا)
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariation> ProductVariations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // مهم جداً عشان يشتغل الـ Identity

            // === 1. Configuring Brands ===
            builder.Entity<Brand>(b =>
            {
                b.HasIndex(b => b.Name).IsUnique(); // اسم البراند فريد
                b.Property(b => b.Name).HasMaxLength(100).IsRequired();
            });

            // === 2. Configuring Categories ===
            builder.Entity<Category>(b =>
            {
                b.HasIndex(c => c.Name).IsUnique(); // اسم الفئة فريد
                b.Property(c => c.Name).HasMaxLength(100).IsRequired();
            });

            // === 3. Configuring Products ===
            builder.Entity<Product>(p =>
            {
                p.HasIndex(p => p.Slug).IsUnique(); // الـ Slug فريد

                // منع مسح البراند أو الفئة لو فيه منتجات مربوطه بيهم (Restrict)
                p.HasOne(p => p.Brand)
                 .WithMany(b => b.Products)
                 .HasForeignKey(p => p.BrandId)
                 .OnDelete(DeleteBehavior.Restrict);

                p.HasOne(p => p.Category)
                 .WithMany(c => c.Products)
                 .HasForeignKey(p => p.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // === 4. Configuring ProductVariations ===
            builder.Entity<ProductVariation>(v =>
            {
                v.HasIndex(v => v.SKU).IsUnique(); // الـ SKU فريد (مطلوب في الـ PDF)
                v.Property(v => v.SKU).HasMaxLength(100).IsRequired();
            });

            // === 5. Configuring Orders ===
            builder.Entity<Order>(o =>
            {
                o.HasIndex(o => o.OrderNumber).IsUnique(); // رقم الطلب فريد

                // تحديد دقة العشرية للفلوس زي ما طلب الـ PDF decimal(18,2)
                o.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
                o.Property(o => o.ShippingCost).HasColumnType("decimal(18,2)");
                o.Property(o => o.DiscountAmount).HasColumnType("decimal(18,2)");
                o.Property(o => o.FinalTotal).HasColumnType("decimal(18,2)");

                // لما يتم مسح العميل، يتم مسح الطلبات بتاعته (أو ممكن Restrict، بس Client هنا Cascade مقبول)
                o.HasOne(o => o.Customer)
                 .WithMany(u => u.Orders)
                 .HasForeignKey(o => o.CustomerId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // === 6. Configuring OrderItems ===
            builder.Entity<OrderItem>(item =>
            {
                // تحديد دقة العشرية للفلوس
                item.Property(item => item.UnitPrice).HasColumnType("decimal(18,2)");
                item.Property(item => item.LineTotal).HasColumnType("decimal(18,2)");
            });
        }
    }
}