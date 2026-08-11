using Microsoft.AspNetCore.Identity;

namespace ClothingStore.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // بيانات إضافية مطلوبة في الـ Registration
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}