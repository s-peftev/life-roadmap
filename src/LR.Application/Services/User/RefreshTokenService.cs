using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Application.Services.User
{
    public class RefreshTokenService(
        IRefreshTokenRepository repository,
        ICancellationTokenProvider cancellationTokenProvider)
        : EntityService<RefreshToken, Guid>(repository, cancellationTokenProvider),
        IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repository = repository;

        protected override Error NotFoundError() =>
            RefreshTokenErrors.NotFound;

        public async Task<Result<RefreshToken>> GetByTokenValueAsync(string refreshTokenValue)
        {
            var ct = _ctProvider.GetCancellationToken();

            var rt = await _repository.GetByTokenValueAsync(refreshTokenValue, ct);

            return rt is null
                ? Result<RefreshToken>.Failure(RefreshTokenErrors.NotFound)
                : Result<RefreshToken>.Success(rt);
        }
    }
}
