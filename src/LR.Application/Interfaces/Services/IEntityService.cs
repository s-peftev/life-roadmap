using LR.Application.AppResult;

namespace LR.Application.Interfaces.Services
{
    public interface IEntityService<TEntity, TKey>
        where TEntity : class
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        Task<Result> RemoveAsync(TKey id);
        Task<Result<TEntity>> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<Result<int>> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
