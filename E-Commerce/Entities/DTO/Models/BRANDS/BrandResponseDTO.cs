
namespace E_Commerce.Entities.DTO.Models.BRANDS
ِnamespace E_Commerce.Entities.DTO.Models.BRANDS

{

    public class BrandResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
