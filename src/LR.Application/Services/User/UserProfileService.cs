using LR.Application.AppResult;
using LR.Application.AppResult.Errors.User;
using LR.Application.AppResult.ResultData.Photo;
using LR.Application.Exceptions.UserProfile;
using LR.Application.Interfaces.ExternalProviders;
using LR.Application.Interfaces.Services;
using LR.Application.Requests.User;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

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

        public async Task<Result<PhotoUploadResult>> UploadProfilePhotoAsync(
            ProfilePhotoUploadRequest request,
            string userId,
            CancellationToken cancellationToken = default)
        { 
            var userProfileResult = await GetByUserIdAsync(userId, cancellationToken);

            if (!userProfileResult.IsSuccess)
                return Result<PhotoUploadResult>.Failure(userProfileResult.Error);

            var uploadResult = await _photoService.UploadPhotoAsync(request.File);

            if (!uploadResult.IsSuccess)
                return uploadResult;

            var userProfile = userProfileResult.Value;
            userProfile.ProfilePhotoUrl = uploadResult.Value.Url;
            userProfile.ProfilePhotoPublicId = uploadResult.Value.PublicId;

            _userProfileRepository.Update(userProfile);
            var saveResult = await _userProfileRepository.SaveChangesAsync(cancellationToken);

            if (saveResult is 0)
                throw new ProfilePersistingException();

            return uploadResult;
        }
    }
}
