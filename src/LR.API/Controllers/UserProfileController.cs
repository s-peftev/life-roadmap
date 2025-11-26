using FluentValidation;
using LR.Application.AppResult.Errors.User;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
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
        IAccountService accountService,
        IUserProfileService userProfileService,
        IValidator<ChangeUsernameRequest> changeUserNameValidator,
        IValidator<ProfilePhotoUploadRequest> profilePhotoUploadValidator,
        IValidator<ChangePersonalInfoRequest> changePersonalInfoValidator) : BaseApiController
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile(CancellationToken ct)
        {
            var myProfileResult = await userProfileService.GetMyProfileAsync(User.GetAppUserId(), ct);

            return myProfileResult.Match(
                data => Ok(ApiResponse<UserProfileDetailsDto>.Ok(data)),
                error => HandleFailure(error)
            );
        }

        [HttpPatch("me")]
        public async Task<IActionResult> ChangeUsername(
            [FromBody] ChangeUsernameRequest changeUsernameRequest,
            CancellationToken ct)
        {
            var validationResult = await changeUserNameValidator.ValidateAsync(changeUsernameRequest, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserProfileErrors.InvalidChangeUsernameRequest with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage) 
                });
            }

            var changeUsernameResult = await accountService.ChangeUsernameAsync(
                changeUsernameRequest,
                User.GetAppUserId());

            return changeUsernameResult.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }

        [HttpPatch("me/personal")]
        public async Task<IActionResult> ChangePersonalInfo(
            [FromBody] ChangePersonalInfoRequest changePersonalInfoRequest,
            CancellationToken ct)
        {
            var validationResult = await changePersonalInfoValidator.ValidateAsync(changePersonalInfoRequest, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserProfileErrors.InvalidChangePersonalInfoRequest with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage) 
                });
            }

            var result = await userProfileService.ChangePersonalInfoAsync(
                User.GetAppUserId(),
                changePersonalInfoRequest,
                ct);

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }

        [HttpPost("photo/upload")]
        public async Task<IActionResult> UploadPhoto(
            [FromForm] ProfilePhotoUploadRequest request,
            CancellationToken ct)
        {
            var validationResult = await profilePhotoUploadValidator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserProfileErrors.InvalidProfilePhotoUploadRequest with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage) 
                });
            }

            var uploadResult = await userProfileService.UploadProfilePhotoAsync(request, User.GetAppUserId(), ct);

            return uploadResult.Match(
                data => Ok(ApiResponse<string>.Ok(data)),
                error => HandleFailure(error)
            );
        }

        [HttpDelete("photo/delete")]
        public async Task<IActionResult> DeletePhoto(CancellationToken ct)
        {
            var deletionResult = await userProfileService.DeleteProfilePhotoAsync(User.GetAppUserId(), ct);

            return deletionResult.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }
    }
}