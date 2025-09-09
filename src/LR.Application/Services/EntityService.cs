using LR.Application.AppResult;
using LR.Application.Interfaces.Services;
using LR.Domain.Interfaces;

namespace LR.Application.Services
{
    public abstract class EntityService<TEntity, TKey>(IRepository<TEntity, TKey> repository)
        : IEntityService<TEntity, TKey>
        where TEntity : class
    {
        private readonly IRepository<TEntity, TKey> _repository = repository;
        private readonly IUnitOfWork _unitOfWork = repository.UoW;

        protected abstract Error NotFoundError();

        public virtual TEntity Add(TEntity entity)
        {
            return _repository.Add(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            return _repository.Update(entity);
        }

        public virtual async Task<Result> RemoveAsync(TKey id, CancellationToken ct = default)
        {
            if (!await _repository.RemoveAsync(id, ct))
                return Result.Failure(NotFoundError());

            return Result.Success();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return await _repository.GetAllAsync(ct);
        }

        public virtual async Task<Result<TEntity>> GetByIdAsync(TKey id, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity is null)
                return Result<TEntity>.Failure(NotFoundError());

            return Result<TEntity>.Success(entity);
        }

        public virtual async Task<Result<int>> SaveChangesAsync(CancellationToken ct = default)
        {
            var changes = await _repository.SaveChangesAsync(ct);

            return Result<int>.Success(changes);
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync(ct);
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            await _unitOfWork.CommitTransactionAsync(ct);
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            await _unitOfWork.RollbackTransactionAsync(ct);
        }
    }
}
