using ClothingStore.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTO.Models.ORDER
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public string Message {  get; set; } = "this operation applied successfully";
        public int StatusCode {  get; set; }     
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public ICollection<OrderItemResponseDTO> OrderItems { get; set; } = new List<OrderItemResponseDTO>();
        [MaxLength(100)]
        public string CustomerFirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string CustomerLastName { get; set; } = string.Empty;
    }
}
