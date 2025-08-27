namespace LR.Domain.Interfaces
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        Task<bool> RemoveAsync(TKey id, CancellationToken ct);
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
