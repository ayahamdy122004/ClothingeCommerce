using ClothingStore.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.Model
{
    public class Brand
    {

   public int Id { get; set; }
        [Required]
       [MaxLength(100)]
        public string Name { get; set; }

        public string ?Description { get; set; }

        public string? LogoUrl { get; set; }

        public bool IsActive { get; set; } = true;


        // Navigation Property
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
