namespace LR.Application.Requests
{
    public abstract class PaginatedRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IReadOnlyList<SortDescriptorDto>? Sort { get; set; } = null;
    }

    public sealed record SortDescriptorDto(string Property, bool Desc);
}
