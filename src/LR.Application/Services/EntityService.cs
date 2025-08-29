using LR.Application.AppResult;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
using LR.Domain.Interfaces;

namespace LR.Application.Services
{
    public abstract class EntityService<TEntity, TKey>(
        IRepository<TEntity, TKey> repository,
        ICancellationTokenProvider cancellationTokenProvider)
        : IEntityService<TEntity, TKey>
        where TEntity : class
    {
        private readonly IRepository<TEntity, TKey> _repository = repository;
        protected readonly ICancellationTokenProvider _ctProvider = cancellationTokenProvider;
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

        public virtual async Task<Result> RemoveAsync(TKey id)
        {
            var ct = _ctProvider.GetCancellationToken();

            if (!await _repository.RemoveAsync(id, ct))
                return Result.Failure(NotFoundError());

            return Result.Success();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var ct = _ctProvider.GetCancellationToken();

            return await _repository.GetAllAsync(ct);
        }

        public virtual async Task<Result<TEntity>> GetByIdAsync(TKey id)
        {
            var ct = _ctProvider.GetCancellationToken();

            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity is null)
                return Result<TEntity>.Failure(NotFoundError());

            return Result<TEntity>.Success(entity);
        }

        public virtual async Task<Result<int>> SaveChangesAsync()
        {
            var ct = _ctProvider.GetCancellationToken();
            var changes = await _repository.SaveChangesAsync(ct);

            return Result<int>.Success(changes);
        }

        public async Task BeginTransactionAsync()
        {
            var ct = _ctProvider.GetCancellationToken();

            await _unitOfWork.BeginTransactionAsync(ct);
        }

        public async Task CommitTransactionAsync()
        {
            var ct = _ctProvider.GetCancellationToken();

            await _unitOfWork.CommitTransactionAsync(ct);
        }

        public async Task RollbackTransactionAsync()
        {
            var ct = _ctProvider.GetCancellationToken();

            await _unitOfWork.RollbackTransactionAsync(ct);
        }
    }
}
