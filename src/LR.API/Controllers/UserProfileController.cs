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
using Swashbuckle.AspNetCore.Annotations;

namespace LR.API.Controllers
{
    [Authorize]
    public class UserProfileController(
        IAccountService accountService,
        IUserProfileService userProfileService,
        IValidator<ChangeUsernameRequest> changeUserNameValidator,
        IValidator<ProfilePhotoUploadRequest> profilePhotoUploadValidator,
        IValidator<ChangePersonalInfoRequest> changePersonalInfoValidator)
        : BaseApiController
    {
        [HttpGet("me")]
        [SwaggerOperation("Get user`s profile", "Returns detailed profile information.")]

        [SwaggerResponse(StatusCodes.Status200OK,       "User`s profile",            typeof(ApiResponse<UserProfileDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User`s profile not found.", typeof(ApiResponse<object>))]
        public async Task<IActionResult> GetMyProfile(CancellationToken ct)
        {
            var result = await userProfileService.GetMyProfileAsync(User.GetAppUserId(), ct);

            return result.Match(
                data => Ok(ApiResponse<UserProfileDetailsDto>.Ok(data)),
                error => HandleFailure(error)
            );
        }

        [HttpPatch("me")]
        [SwaggerOperation("Change username", "Updates the user`s username.")]

        [SwaggerResponse(StatusCodes.Status200OK,         "Username has been changed.", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation errors",          typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status409Conflict,   "Username is taken",          typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound,   "User`s profile not found.",  typeof(ApiResponse<object>))]
        public async Task<IActionResult> ChangeUsername([FromBody] ChangeUsernameRequest changeUsernameRequest, CancellationToken ct)
        {
            var validationResult = await changeUserNameValidator.ValidateAsync(changeUsernameRequest, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserProfileErrors.InvalidChangeUsernameRequest with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage) 
                });
            }

            var result = await accountService.ChangeUsernameAsync(changeUsernameRequest, User.GetAppUserId());

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }

        [HttpPatch("me/personal")]
        [SwaggerOperation("Change personal info", "Updates the user`s personal info.")]

        [SwaggerResponse(StatusCodes.Status200OK,         "Personal info has been changed.", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation errors",               typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound,   "User`s profile not found.",       typeof(ApiResponse<object>))]
        public async Task<IActionResult> ChangePersonalInfo([FromBody] ChangePersonalInfoRequest changePersonalInfoRequest, CancellationToken ct)
        {
            var validationResult = await changePersonalInfoValidator.ValidateAsync(changePersonalInfoRequest, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserProfileErrors.InvalidChangePersonalInfoRequest with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage) 
                });
            }

            var result = await userProfileService.ChangePersonalInfoAsync(User.GetAppUserId(), changePersonalInfoRequest, ct);

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }

        [HttpPost("photo/upload")]
        [SwaggerOperation("Upload profile photo", "Sets the user`s new profile photo.")]

        [SwaggerResponse(StatusCodes.Status200OK,                  "Profile photo has been uploaded.",     typeof(ApiResponse<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,          "Validation errors",                    typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Failed to upload user profile photo.", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable,  "Service is unavailable.",              typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound,            "User`s profile not found.",            typeof(ApiResponse<object>))]
        public async Task<IActionResult> UploadPhoto([FromForm] ProfilePhotoUploadRequest request, CancellationToken ct)
        {
            var validationResult = await profilePhotoUploadValidator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserProfileErrors.InvalidProfilePhotoUploadRequest with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage) 
                });
            }

            var result = await userProfileService.UploadProfilePhotoAsync(request, User.GetAppUserId(), ct);

            return result.Match(
                data => Ok(ApiResponse<string>.Ok(data)),
                error => HandleFailure(error)
            );
        }

        [HttpDelete("photo/delete")]
        [SwaggerOperation("Delete user`s profile photo", "Deletes the user's profile photo.")]

        [SwaggerResponse(StatusCodes.Status200OK,                  "User`s profile photo has been deleted.", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Failed to delete user`s profile photo.", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable,  "Service is unavailable.",                typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound,            "User`s profile not found.",              typeof(ApiResponse<object>))]
        public async Task<IActionResult> DeletePhoto(CancellationToken ct)
        {
            var result = await userProfileService.DeleteProfilePhotoAsync(User.GetAppUserId(), ct);

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }
    }
}