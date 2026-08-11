using ClothingStore.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.Model
{
    public class Category
    {

    public int Id { get; set; }
        [Required]
            [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }


        // Navigation Property
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}