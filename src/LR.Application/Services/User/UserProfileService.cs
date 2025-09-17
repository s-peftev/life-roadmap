using LR.Application.AppResult;
using LR.Application.AppResult.Errors.User;
using LR.Application.Exceptions.UserProfile;
using LR.Application.Interfaces.ExternalProviders;
using LR.Application.Interfaces.Services;
using LR.Application.Requests.User;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;
using LR.Domain.ValueObjects.UserProfile;

namespace LR.Application.Services.User
{
    public class UserProfileService(
        IUserProfileRepository repository,
        IPhotoService photoService)
        : EntityService<UserProfile, Guid>(repository),
        IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository = repository;
        private readonly IPhotoService _photoService = photoService;

        protected override Error NotFoundError() => 
            UserProfileErrors.NotFound;

        public async Task<Result<UserProfile>> GetByUserIdAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId, cancellationToken);

            return userProfile is null
                ? Result<UserProfile>.Failure(UserProfileErrors.NotFound)
                : Result<UserProfile>.Success(userProfile);
        }

        public async Task<Result<string>> UploadProfilePhotoAsync(
            ProfilePhotoUploadRequest request,
            string userId,
            CancellationToken cancellationToken = default)
        { 
            var userProfileResult = await GetByUserIdAsync(userId, cancellationToken);

            if (!userProfileResult.IsSuccess)
                return Result<string>.Failure(userProfileResult.Error);

            var uploadResult = await _photoService.UploadPhotoAsync(request.File);

            if (!uploadResult.IsSuccess)
                return Result<string>.Failure(uploadResult.Error);

            var userProfile = userProfileResult.Value;
            userProfile.ProfilePhotoUrl = uploadResult.Value.Url;
            userProfile.ProfilePhotoPublicId = uploadResult.Value.PublicId;
            userProfile.UpdatedAt = DateTime.UtcNow;

            var saveResult = await _userProfileRepository.SaveChangesAsync(cancellationToken);

            if (saveResult is 0)
                throw new ProfilePersistingException();

            return Result<string>.Success(uploadResult.Value.Url);
        }

        public async Task<Result<UserProfileDetailsDto>> GetMyProfileAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var profileDetails = await _userProfileRepository.GetProfileProfileDetailsAsync(userId, cancellationToken);

            return profileDetails is null
                ? Result<UserProfileDetailsDto>.Failure(UserProfileErrors.NotFound)
                : Result<UserProfileDetailsDto>.Success(profileDetails);
        }

        public async Task<Result> DeleteProfilePhotoAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var userProfileResult = await GetByUserIdAsync(userId, cancellationToken);

            if (!userProfileResult.IsSuccess)
                return Result.Failure(userProfileResult.Error);

            if (string.IsNullOrEmpty(userProfileResult.Value.ProfilePhotoPublicId))
                return Result.Success();

            var deletionResult = await _photoService.DeletePhotoAsync(userProfileResult.Value.ProfilePhotoPublicId);

            if (deletionResult.IsSuccess)
            {
                var userProfile = userProfileResult.Value;

                userProfile.ProfilePhotoUrl = null;
                userProfile.ProfilePhotoPublicId = null;
                userProfile.UpdatedAt = DateTime.UtcNow;

                var saveResult = await _userProfileRepository.SaveChangesAsync(cancellationToken);

                if (saveResult is 0)
                    throw new ProfilePersistingException();
            }

            return deletionResult;
        }

        public async Task<Result> ChangePersonalInfoAsync(
            string userId,
            ChangePersonalInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            var userProfileResult = await GetByUserIdAsync(userId, cancellationToken);

            if (!userProfileResult.IsSuccess)
                return Result.Failure(userProfileResult.Error);

            var userProfile = userProfileResult.Value;

            userProfile.FirstName = request.FirstName;
            userProfile.LastName = request.LastName;
            userProfile.BirthDate = request.BirthDate;
            userProfile.UpdatedAt = DateTime.UtcNow;

            var saveResult = await _userProfileRepository.SaveChangesAsync(cancellationToken);

            if (saveResult is 0)
                throw new ProfilePersistingException();

            return Result.Success();
        }
    }
}
