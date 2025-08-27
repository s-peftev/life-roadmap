using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Application.Interfaces.Services;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;

namespace LR.Application.Services.User
{
    public class RefreshTokenService(
        IRefreshTokenRepository repository,
        IHttpContextAccessor httpContextAccessor)
        : EntityService<RefreshToken, Guid>(repository, httpContextAccessor),
        IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repository = repository;

        protected override Error NotFoundError() =>
            RefreshTokenErrors.NotFound;

        protected override Error SaveFailedError() =>
            RefreshTokenErrors.SaveFailed;

        public async Task<RefreshToken?> GetByTokenValueAsync(
            string refreshTokenValue, CancellationToken ct = default)
        {
            var cancellationToken = ResolveCancellationToken(ct);

            return await _repository.GetByTokenValueAsync(refreshTokenValue, cancellationToken);
        }
    }
}
