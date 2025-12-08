using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Application.AppResult.ResultData.Photo;
using LR.Application.Interfaces.ExternalProviders;
using LR.Application.Interfaces.Services;
using LR.Application.Requests.User;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace LR.Application.Services.User
{
    public class UserProfileService(
        ILogger<UserProfileService> logger,
        IUserProfileRepository userProfileRepository,
        IPhotoService photoService) 
        : EntityService<UserProfile, Guid>(userProfileRepository), IUserProfileService
    {
        public async Task<Result<string>> UploadProfilePhotoAsync(ProfilePhotoUploadRequest request, string userId, CancellationToken ct = default)
        { 
            var userProfileResult = await GetByUserIdAsync(userId, ct);

            if (!userProfileResult.IsSuccess)
                return Result<string>.Failure(userProfileResult.Error);

            var userProfile = userProfileResult.Value;
            Result<PhotoUploadResult> uploadResult;

            try
            {
                uploadResult = await photoService.UploadPhotoAsync(request.File);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Uploading photo to cloud failed");

                return Result<string>.Failure(GeneralErrors.ServiceUnavailable);
            }

            if (!uploadResult.IsSuccess) 
                return Result<string>.Failure(uploadResult.Error);

            userProfile.ProfilePhotoUrl = uploadResult.Value.Url;
            userProfile.ProfilePhotoPublicId = uploadResult.Value.PublicId;

            try 
            {
                await userProfileRepository.SaveChangesAsync(ct);

                return Result<string>.Success(uploadResult.Value.Url);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Persisting uploaded profile photo failed");

                try
                {
                    await photoService.DeletePhotoAsync(uploadResult.Value.PublicId);
                }
                catch (Exception deleteEx)
                {
                    logger.LogError(deleteEx, "Rollback photo delete failed");
                }

                return Result<string>.Failure(GeneralErrors.InternalServerError);
            }
        }

        public async Task<Result<UserProfile>> GetByUserIdAsync(string userId, CancellationToken ct = default)
        {
            var userProfile = await userProfileRepository.GetByUserIdAsync(userId, ct);

            return userProfile is null
                ? Result<UserProfile>.Failure(GeneralErrors.NotFound)
                : Result<UserProfile>.Success(userProfile);
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

            try
            {
                await userProfileRepository.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to persist personal info changings");

                return Result.Failure(GeneralErrors.InternalServerError);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteProfilePhotoAsync(string userId, CancellationToken ct = default)
        {
            var userProfileResult = await GetByUserIdAsync(userId, ct);

            if (!userProfileResult.IsSuccess)
                return Result.Failure(userProfileResult.Error);

            var userProfile = userProfileResult.Value;
            var publicId = userProfile.ProfilePhotoPublicId;

            if (string.IsNullOrEmpty(publicId))
                return Result.Success();

            userProfile.ProfilePhotoUrl = null;
            userProfile.ProfilePhotoPublicId = null;

            try
            {
                await userProfileRepository.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Profile photo deletion failed");

                return Result.Failure(GeneralErrors.InternalServerError);
            }

            try
            {
                var deletionResult = await photoService.DeletePhotoAsync(publicId);

                if (!deletionResult.IsSuccess)
                    logger.LogError("Cloud deletion failed: {Error}", deletionResult.Error.Description);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Cloud deletion exception");
            }

            return Result.Success();
        }
    }
}