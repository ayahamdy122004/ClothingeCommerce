namespace E_Commerce.Entities.DTO.Models.PRODUCTIMAGEFolder
{
    public class UploadImageDTO
    {
        public int ProductId { get; set; }
        public IFormFile File { get; set; } = null!;
        public string? AlternativeText { get; set; }
        public bool IsCover { get; set; } = false;
    }
}
