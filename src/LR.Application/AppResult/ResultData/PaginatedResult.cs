using LR.Domain.Common.Models;

namespace LR.Application.AppResult.ResultData
{
    public class PaginatedResult<T>
    {
        public PaginationMetadata Metadata { get; init; } = new PaginationMetadata();
        public IEnumerable<T> Items { get; init; } = [];

        public PaginatedResult(RepositoryPagedResult<T> repositoryPagedResult)
        {
            Metadata = new PaginationMetadata
            {
                CurrentPage = repositoryPagedResult.PageNumber,
                PageSize = repositoryPagedResult.PageSize,
                TotalCount = repositoryPagedResult.TotalCount,
                TotalPages = (int)Math.Ceiling(repositoryPagedResult.TotalCount / (double)repositoryPagedResult.PageSize)
            };

            Items = repositoryPagedResult.Items;
        }
    }

    public class PaginationMetadata
    {
        public int CurrentPage { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
        public int TotalCount { get; init; }
    }
}
