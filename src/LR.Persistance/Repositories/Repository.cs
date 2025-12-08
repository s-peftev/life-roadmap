using LR.Domain.Common.Models;
using LR.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LR.Persistance.Repositories
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        public IUnitOfWork UoW { get; }

        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context, IUnitOfWork unitOfWork)
        {
            UoW = unitOfWork;
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

        public virtual async Task<RepositoryPagedResult<TItem>> GetPagedAsync<TItem>(IQueryable<TItem> query, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var totalCount = await query.CountAsync(ct);

            var pagedList = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new RepositoryPagedResult<TItem>(
                Items: pagedList,
                TotalCount: totalCount,
                PageNumber: pageNumber,
                PageSize: pageSize
            );
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync([id], cancellationToken: ct);
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}