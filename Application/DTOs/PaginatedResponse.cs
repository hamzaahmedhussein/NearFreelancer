namespace Connect.Application.DTOs
{
    public class PaginatedResponse<T>
    {
        public IReadOnlyList<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

}
