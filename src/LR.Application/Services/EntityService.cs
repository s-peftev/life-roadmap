using LR.Application.AppResult;
using LR.Application.Interfaces.Services;
using LR.Domain.Interfaces;

namespace LR.Application.Services
{
    public abstract class EntityService<TEntity, TKey>(IRepository<TEntity, TKey> repository)
        : IEntityService<TEntity, TKey>
        where TEntity : class
    {
        protected readonly IRepository<TEntity, TKey> _repository = repository;

        protected abstract Error NotFoundError();
        protected abstract Error SaveFailedError();

        public virtual TEntity Add(TEntity entity)
        {
            return _repository.Add(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            return _repository.Update(entity);
        }

        public virtual async Task<Result> RemoveAsync(TKey id)
        {
            if (!await _repository.RemoveAsync(id))
                return Result.Failure(NotFoundError());

            return Result.Success();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<Result<TEntity>> GetByIdAsync(TKey id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity is null)
                return Result<TEntity>.Failure(NotFoundError());

            return Result<TEntity>.Success(entity);
        }

        public virtual async Task<Result<int>> SaveChangesAsync()
        {
            var changes = await _repository.SaveChangesAsync();
            if (changes == 0)
                return Result<int>.Failure(SaveFailedError());

            return Result<int>.Success(changes);
        }
    }
}
