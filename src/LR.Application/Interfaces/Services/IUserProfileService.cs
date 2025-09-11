using LR.Application.AppResult;
using LR.Domain.Entities.Users;

namespace LR.Application.Interfaces.Services
{
    public interface IUserProfileService : IEntityService<UserProfile, Guid>
    {
        Task<Result<UserProfile>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
