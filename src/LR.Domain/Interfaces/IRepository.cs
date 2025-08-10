namespace LR.Domain.Interfaces
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        Task DeleteAsync(TKey id);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<int> SaveChangesAsync();
    }
}
