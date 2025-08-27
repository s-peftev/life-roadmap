using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Application.Interfaces.Services;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;

namespace LR.Application.Services.User
{
    public class UserProfileService(
        IUserProfileRepository repository,
        IHttpContextAccessor httpContextAccessor)
        : EntityService<UserProfile, Guid>(repository, httpContextAccessor),
        IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository = repository;

        protected override Error NotFoundError() => 
            UserProfileErrors.NotFound;

        protected override Error SaveFailedError() => 
            UserProfileErrors.SaveFailed;
    }
}
