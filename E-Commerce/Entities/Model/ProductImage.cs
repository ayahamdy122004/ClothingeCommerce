using ClothingStore.Entities;

namespace E_Commerce.Entities.Model
{
    public class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ImageUrl { get; set; }

        public string AlternativeText { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsCover { get; set; }


        // Navigation Property

        public Product Product { get; set; }
    }
}