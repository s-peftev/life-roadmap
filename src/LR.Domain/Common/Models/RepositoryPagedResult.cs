namespace LR.Domain.Common.Models
{
    public sealed record RepositoryPagedResult<T>(
        IReadOnlyList<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize
    );
}
