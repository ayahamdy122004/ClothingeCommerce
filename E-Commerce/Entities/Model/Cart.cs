namespace E_Commerce.Entities.Model
{
    public class Cart
    {
        public string UserId { get; set; }  
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
