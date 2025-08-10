using LR.Application.Interfaces.Services;
using LR.Domain.Interfaces;

namespace LR.Application.Services
{
    public class EntityService<TEntity, TKey>(IRepository<TEntity, TKey> repository) 
        : IEntityService<TEntity, TKey>
        where TEntity : class
    {
        protected readonly IRepository<TEntity, TKey> _repository = repository;

        public TEntity Add(TEntity entity)
        {
            return  _repository.Add(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            await _repository.DeleteAsync(id);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<TEntity?> GetByIdAsync(TKey id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<int> SaveChangesAsync()
        {
            return _repository.SaveChangesAsync();
        }

        public TEntity Update(TEntity entity)
        {
            return _repository.Update(entity);
        }
    }
}
