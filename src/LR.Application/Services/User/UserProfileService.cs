using LR.Application.Interfaces.Services;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Application.Services.User
{
    public class UserProfileService(IUserProfileRepository repository)
        : EntityService<UserProfile, Guid>(repository), IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository = repository;
    }
}
