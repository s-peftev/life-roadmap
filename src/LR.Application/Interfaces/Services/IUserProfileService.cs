using LR.Application.AppResult;
using LR.Application.Requests.User;
using LR.Domain.Entities.Users;
using LR.Domain.ValueObjects.UserProfile;

namespace LR.Application.Interfaces.Services
{
    public interface IUserProfileService : IEntityService<UserProfile, Guid>
    {
        Task<Result<UserProfile>> GetByUserIdAsync(string userId, CancellationToken ct = default);
        Task<Result<UserProfileDetailsDto>> GetMyProfileAsync(string userId, CancellationToken ct = default);
        Task<Result<string>> UploadProfilePhotoAsync(ProfilePhotoUploadRequest request, string userId, CancellationToken ct = default);
        Task<Result> DeleteProfilePhotoAsync(string userId, CancellationToken ct = default);
        Task<Result> ChangePersonalInfoAsync(string userId, ChangePersonalInfoRequest request, CancellationToken ct = default);
    }
}
