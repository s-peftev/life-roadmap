using LR.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LR.Persistance.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> 
        where TEntity : class
    {
        private readonly AppDbContext _context;
        protected DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            _dbSet.Add(entity);

            return entity;
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id) 
                ?? throw new KeyNotFoundException($"Entity with id {id} not found.");

            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public TEntity Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }
    }
}
