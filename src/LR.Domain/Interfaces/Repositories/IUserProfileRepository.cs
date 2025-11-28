using LR.Domain.Entities.Users;
using LR.Domain.ValueObjects.UserProfile;

namespace LR.Domain.Interfaces.Repositories
{
    public interface IUserProfileRepository : IRepository<UserProfile, Guid>
    {
        Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken ct = default);
        Task<UserProfileDetailsDto?> GetProfileDetailsAsync(string userId, CancellationToken ct = default);
    }
}
