namespace E_Commerce.Entities.DTO.Models.CART
{
    public class AddCartDTO
    {
        public int ProductId { get; set; }
        public int ProductVariationId { get; set; }
        public int ProductImageId { get; set; } 
      
        public int Quantity { get; set; }
    }
}
