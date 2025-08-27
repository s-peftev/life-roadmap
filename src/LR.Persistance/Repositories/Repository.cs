using LR.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LR.Persistance.Repositories
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual TEntity Add(TEntity entity)
        {
            _dbSet.Add(entity);

            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            _dbSet.Update(entity);

            return entity;
        }

        public virtual async Task<bool> RemoveAsync(TKey id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);

            if (entity is null) 
                return false;

            _dbSet.Remove(entity);

            return true;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbSet.ToListAsync(ct);
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(id, ct);
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}