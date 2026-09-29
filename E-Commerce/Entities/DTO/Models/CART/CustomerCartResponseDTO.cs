using E_Commerce.Entities.DTO.Models.CART;

public class CustomerCartResponseDTO
{
    public string UserId { get; set; } = string.Empty;
    public DateTime LastUpdatedDate { get; set; }
    public List<CartItemResponseDTO> Items { get; set; } = new List<CartItemResponseDTO>();

    // الحسابات المطلوبة من المنتور:
    public int TotalUnits { get; set; }
    public decimal SubTotal { get; set; }
    public decimal EstimatedShipping { get; set; }
    public decimal EstimatedFinalTotal { get; set; }




}