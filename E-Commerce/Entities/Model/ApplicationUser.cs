using Microsoft.AspNetCore.Identity;

namespace ClothingStore.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

      
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Governance { get; set; } // المحافظة
        public string? PostalCode { get; set; }

       
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}