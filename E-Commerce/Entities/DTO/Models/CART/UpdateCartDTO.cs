namespace E_Commerce.Entities.DTO.Models.CART
{
    public class UpdateCartDTO
    {
        public int ProductVariationId { get; set; } // تأكدي إنها ProductVariationId وليس ProductId
        public int Quantity { get; set; }
       
    }
}
