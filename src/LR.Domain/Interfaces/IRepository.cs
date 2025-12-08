using LR.Domain.Common.Models;

namespace LR.Domain.Interfaces
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        IUnitOfWork UoW { get; }
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        Task<bool> RemoveAsync(TKey id, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
        Task<RepositoryPagedResult<TItem>> GetPagedAsync<TItem>(IQueryable<TItem> query, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}