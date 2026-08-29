namespace E_Commerce.Entities.DTO.Models.PRODUCTS.ProductFilterAndSearch
{
    public class ProductQueryDTO
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public int? BrandId { get; set; }

        public string? Size { get; set; }

        public string? Color { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public bool? InStockOnly { get; set; }

        public bool? IsFeatured { get; set; }

        public string? Sort { get; set; }
    }
}