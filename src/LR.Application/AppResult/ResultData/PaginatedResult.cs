namespace LR.Application.AppResult.ResultData
{
    public class PaginatedResult<T>
    {
        public PaginationMetadata Metadata { get; set; } = new PaginationMetadata();
        public IEnumerable<T> Items { get; set; } = [];
    }

    public class PaginationMetadata
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
