using LR.Application.AppResult;
using LR.Application.AppResult.Errors.User;
using LR.Application.Interfaces.Services;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Application.Services.User
{
    public class RefreshTokenService(
        IRefreshTokenRepository repository)
        : EntityService<RefreshToken, Guid>(repository),
        IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repository = repository;

        protected override Error NotFoundError() =>
            RefreshTokenErrors.NotFound;

        public async Task<Result<RefreshToken>> GetByTokenValueAsync(
            string refreshTokenValue,
            CancellationToken cancellationToken = default)
        {
            var rt = await _repository.GetByTokenValueAsync(refreshTokenValue, cancellationToken);

            return rt is null
                ? Result<RefreshToken>.Failure(RefreshTokenErrors.NotFound)
                : Result<RefreshToken>.Success(rt);
        }
    }
}
