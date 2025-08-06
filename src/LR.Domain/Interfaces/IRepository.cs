namespace LR.Domain.Interfaces
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TKey id);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<int> SaveChangesAsync();
    }
}
