using LR.Application.AppResult;
using LR.Application.Interfaces.Services;
using LR.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LR.Application.Services
{
    public abstract class EntityService<TEntity, TKey>(
        IRepository<TEntity, TKey> repository,
        IHttpContextAccessor httpContextAccessor)
        : IEntityService<TEntity, TKey>
        where TEntity : class
    {
        private readonly IRepository<TEntity, TKey> _repository = repository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        protected abstract Error NotFoundError();
        protected abstract Error SaveFailedError();

        // Uses the CancellationToken from HttpContext as a fallback so that
        // service methods can automatically pick up the cancellation token of the web request,
        // without having to pass it from the controller all the way down to the repository.
        // If a token is provided explicitly, it takes priority.
        protected virtual CancellationToken ResolveCancellationToken(CancellationToken ct = default) =>
            ct != default ? ct : _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

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
            var cancellationToken = ResolveCancellationToken(ct);

            if (!await _repository.RemoveAsync(id, cancellationToken))
                return Result.Failure(NotFoundError());

            return Result.Success();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
        {
            var cancellationToken = ResolveCancellationToken(ct);

            return await _repository.GetAllAsync(cancellationToken);
        }

        public virtual async Task<Result<TEntity>> GetByIdAsync(TKey id, CancellationToken ct = default)
        {
            var cancellationToken = ResolveCancellationToken(ct);

            var entity = await _repository.GetByIdAsync(id, cancellationToken);

            if (entity is null)
                return Result<TEntity>.Failure(NotFoundError());

            return Result<TEntity>.Success(entity);
        }

        public virtual async Task<Result<int>> SaveChangesAsync(CancellationToken ct = default)
        {
            var cancellationToken = ResolveCancellationToken(ct);
            var changes = await _repository.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(changes);
        }
    }
}
