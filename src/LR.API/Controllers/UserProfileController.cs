using FluentValidation;
using LR.Application.AppResult.Errors.User;
using LR.Application.Interfaces.Services;
using LR.Application.Requests.User;
using LR.Application.Responses;
using LR.Domain.ValueObjects.UserProfile;
using LR.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LR.API.Controllers
{
    [Authorize]
    public class UserProfileController(
        IUserProfileService userProfileService,
        IValidator<ProfilePhotoUploadRequest> profilePhotoUploadValidator) : BaseApiController
    {
        private readonly IUserProfileService _userProfileService = userProfileService;
        private readonly IValidator<ProfilePhotoUploadRequest> _profilePhotoUploadValidator = profilePhotoUploadValidator;

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var myProfileResult = await _userProfileService.GetMyProfileAsync(User.GetAppUserId());

            return myProfileResult.Match(
                data => Ok(ApiResponse<UserProfileDetailsDto>.Ok(data)),
                error => HandleFailure(error));
        }

        [HttpPost("photo/upload")]
        public async Task<IActionResult> UploadPhoto(
            [FromForm] ProfilePhotoUploadRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _profilePhotoUploadValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserProfileErrors.InvalidProfilePhotoUploadRequest 
                    with { Details = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var uploadResult = await _userProfileService.UploadProfilePhotoAsync(request, User.GetAppUserId(), cancellationToken);

            return uploadResult.Match(
                data => Ok(ApiResponse<string>.Ok(data)),
                error => HandleFailure(error));
        }

        [HttpDelete("photo/delete")]
        public async Task<IActionResult> DeletePhoto(CancellationToken cancellationToken)
        { 
            var deletionResult = await _userProfileService.DeleteProfilePhotoAsync(User.GetAppUserId(), cancellationToken);

            return deletionResult.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error));
        }
    }
}
