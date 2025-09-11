using LR.Application.AppResult;
using LR.Application.AppResult.Errors.User;
using LR.Application.Interfaces.Services;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Application.Services.User
{
    public class UserProfileService(
        IUserProfileRepository repository)
        : EntityService<UserProfile, Guid>(repository),
        IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository = repository;

        protected override Error NotFoundError() => 
            UserProfileErrors.NotFound;

        public async Task<Result<UserProfile>> GetByUserIdAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId, cancellationToken);

            return userProfile is null
                ? Result<UserProfile>.Failure(UserProfileErrors.NotFound)
                : Result<UserProfile>.Success(userProfile);
        }
    }
}
