using LR.Application.AppResult;
using LR.Application.AppResult.Errors.User;
using LR.Application.Exceptions.UserProfile;
using LR.Application.Interfaces.ExternalProviders;
using LR.Application.Interfaces.Services;
using LR.Application.Requests.User;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;
using LR.Domain.ValueObjects.UserProfile;
using Microsoft.Extensions.Logging;

namespace LR.Application.Services.User
{
    public class UserProfileService(
        ILogger<UserProfileService> logger,
        IUserProfileRepository userProfileRepository,
        IPhotoService photoService) 
        : EntityService<UserProfile, Guid>(userProfileRepository), IUserProfileService
    {
        protected override Error NotFoundError() => 
            UserProfileErrors.NotFound;

        public async Task<Result<string>> UploadProfilePhotoAsync(ProfilePhotoUploadRequest request, string userId, CancellationToken ct = default)
        { 
            var userProfileResult = await GetByUserIdAsync(userId, ct);

            if (!userProfileResult.IsSuccess)
                return Result<string>.Failure(userProfileResult.Error);

            var uploadResult = await photoService.UploadPhotoAsync(request.File);

            if (!uploadResult.IsSuccess)
                return Result<string>.Failure(uploadResult.Error);

            var userProfile = userProfileResult.Value;
            userProfile.ProfilePhotoUrl = uploadResult.Value.Url;
            userProfile.ProfilePhotoPublicId = uploadResult.Value.PublicId;
            userProfile.UpdatedAt = DateTime.UtcNow;

            var saveResult = await userProfileRepository.SaveChangesAsync(ct);

            if (saveResult is 0)
                throw new ProfilePersistingException();

            return Result<string>.Success(uploadResult.Value.Url);
        }

        public async Task<Result<UserProfile>> GetByUserIdAsync(string userId, CancellationToken ct = default)
        {
            var userProfile = await userProfileRepository.GetByUserIdAsync(userId, ct);

            return userProfile is null
                ? Result<UserProfile>.Failure(UserProfileErrors.NotFound)
                : Result<UserProfile>.Success(userProfile);
        }

        public async Task<Result<UserProfileDetailsDto>> GetMyProfileAsync(string userId, CancellationToken ct = default)
        {
            var profileDetails = await userProfileRepository.GetProfileDetailsAsync(userId, ct);

            return profileDetails is null
                ? Result<UserProfileDetailsDto>.Failure(UserProfileErrors.NotFound)
                : Result<UserProfileDetailsDto>.Success(profileDetails);
        }

        public async Task<Result> ChangePersonalInfoAsync(string userId, ChangePersonalInfoRequest request, CancellationToken ct = default)
        {
            var userProfileResult = await GetByUserIdAsync(userId, ct);

            if (!userProfileResult.IsSuccess)
                return Result.Failure(userProfileResult.Error);

            var userProfile = userProfileResult.Value;

            userProfile.FirstName = request.FirstName;
            userProfile.LastName = request.LastName;
            userProfile.BirthDate = request.BirthDate;
            userProfile.UpdatedAt = DateTime.UtcNow;

            var saveResult = await userProfileRepository.SaveChangesAsync(ct);

            if (saveResult is 0)
                throw new ProfilePersistingException();

            return Result.Success();
        }

        public async Task<Result> DeleteProfilePhotoAsync(string userId, CancellationToken ct = default)
        {
            var userProfileResult = await GetByUserIdAsync(userId, ct);

            if (!userProfileResult.IsSuccess)
                return Result.Failure(userProfileResult.Error);

            if (string.IsNullOrEmpty(userProfileResult.Value.ProfilePhotoPublicId))
                return Result.Success();

            var userProfile = userProfileResult.Value;

            try
            {
                var publicId = userProfile.ProfilePhotoPublicId;

                userProfile.ProfilePhotoUrl = null;
                userProfile.ProfilePhotoPublicId = null;
                userProfile.UpdatedAt = DateTime.UtcNow;

                await userProfileRepository.SaveChangesAsync(ct);

                var deletionResult = await photoService.DeletePhotoAsync(publicId);

                return deletionResult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Profile photo deletion failed");

                return Result.Failure(PhotoErrors.ServiceUnavailable);
            }
        }
    }
}