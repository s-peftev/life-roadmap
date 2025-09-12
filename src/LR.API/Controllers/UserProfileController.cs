using FluentValidation;
using LR.Application.AppResult.Errors.User;
using LR.Application.AppResult.ResultData.Photo;
using LR.Application.Interfaces.ExternalProviders;
using LR.Application.Interfaces.Services;
using LR.Application.Requests.User;
using LR.Application.Responses;
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
                data => Ok(ApiResponse<PhotoUploadResult>.Ok(data)),
                error => HandleFailure(error));
        }
    }
}
