namespace E_Commerce.Entities.DTO.Models.Common
{
    public class PaginatedResponseDTO<ProductResponseDTO>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;
        public IEnumerable<ProductResponseDTO> Data { get; set; } = new List<ProductResponseDTO>();

        public PaginatedResponseDTO(int currentPage, int pageSize, int 
            totalRecords, IEnumerable<ProductResponseDTO> data)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            Data = data;
        }
    }
}