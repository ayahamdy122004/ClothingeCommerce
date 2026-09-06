namespace E_Commerce.Entities.DTO.Models.ORDER
{
    public class CheckoutDTO
    {
        // بيانات عنوان الشحن المطلوبة
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        // أسلوب الشحن أو طريقة الدفع حسب المطلوب من المنتور
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
