using LR.Application.AppResult;

namespace LR.Application.Interfaces.Services
{
    public interface IEntityService<TEntity, TKey>
        where TEntity : class
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        Task<Result> RemoveAsync(TKey id, CancellationToken ct = default);
        Task<Result<TEntity>> GetByIdAsync(TKey id, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
        Task<Result<int>> SaveChangesAsync(CancellationToken ct = default);
    }
}
