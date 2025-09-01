using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Application.Services.User
{
    public class UserProfileService(
        IUserProfileRepository repository,
        ICancellationTokenProvider cancellationTokenProvider)
        : EntityService<UserProfile, Guid>(repository, cancellationTokenProvider),
        IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository = repository;

        protected override Error NotFoundError() => 
            UserProfileErrors.NotFound;
    }
}
