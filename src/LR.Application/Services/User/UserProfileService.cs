using LR.Application.AppResult;
using LR.Application.AppResult.Errors.User;
using LR.Application.AppResult.ResultData.Photo;
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

            Result<PhotoUploadResult>? uploadResult = null;

            try 
            {
                uploadResult = await photoService.UploadPhotoAsync(request.File);

                if (!uploadResult.IsSuccess)
                    return Result<string>.Failure(uploadResult.Error);

                var userProfile = userProfileResult.Value;
                userProfile.ProfilePhotoUrl = uploadResult.Value.Url;
                userProfile.ProfilePhotoPublicId = uploadResult.Value.PublicId;
                userProfile.UpdatedAt = DateTime.UtcNow;

                await userProfileRepository.SaveChangesAsync(ct);

                return Result<string>.Success(uploadResult.Value.Url);

            }
            catch (Exception ex)
            {
                if (uploadResult is not null && uploadResult.IsSuccess)
                {
                    try
                    {
                        // Rollback uploaded photo if saving profile failed
                        await photoService.DeletePhotoAsync(uploadResult.Value.PublicId);
                    }
                    catch(Exception deleteEx)
                    {
                        logger.LogError(deleteEx, "Rollback photo delete failed");
                    }
                }

                logger.LogError(ex, "Profile photo upload failed");

                return Result<string>.Failure(PhotoErrors.ServiceUnavailable);
            }
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

                Result deletionResult;

                try
                {
                    deletionResult = await photoService.DeletePhotoAsync(publicId);

                    if (!deletionResult.IsSuccess)
                        logger.LogError("Cloud deletion failed: {Error}", deletionResult.Error.Description);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Cloud deletion exception");
                }

                // consider photo deletion success even if cloud deletion failed
                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Profile photo deletion failed");

                return Result.Failure(PhotoErrors.ServiceUnavailable);
            }
        }
    }
}