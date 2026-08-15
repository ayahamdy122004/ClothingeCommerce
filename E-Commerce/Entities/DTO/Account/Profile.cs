namespace E_Commerce.Entities.DTO.Account
{
    // 5. لعرض بيانات البروفايل (Response)
    public class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

