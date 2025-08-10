using LR.Application.Interfaces.Services;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Application.Services
{
    public class UserProfileService(IUserProfileRepository repository) 
        : EntityService<UserProfile, Guid>(repository), IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository = repository;

        public async Task<UserProfile?> GetByUserProfileByRefreshTokenAsync(string refreshToken)
        {
            var userProfile = await _userProfileRepository
                .GetByUserProfileByRefreshTokenAsync(refreshToken);

            return userProfile;
        }
    }
}
