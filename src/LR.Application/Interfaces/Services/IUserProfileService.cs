using LR.Application.AppResult;
using LR.Application.AppResult.ResultData.Photo;
using LR.Application.Requests.User;
using LR.Domain.Entities.Users;

namespace LR.Application.Interfaces.Services
{
    public interface IUserProfileService : IEntityService<UserProfile, Guid>
    {
        Task<Result<UserProfile>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<Result<string>> UploadProfilePhotoAsync(
            ProfilePhotoUploadRequest request,
            string userId,
            CancellationToken cancellationToken = default);
    }
}
