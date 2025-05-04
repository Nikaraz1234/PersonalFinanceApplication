namespace PersonalFinanceApplication.DTOs.Pagination
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public string? SortBy { get; set; } = "date";
        public bool IsDescending { get; set; } = true;
    }
}
