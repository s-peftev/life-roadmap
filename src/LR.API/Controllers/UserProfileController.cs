using FluentValidation;
using LR.Application.AppResult.Errors.User;
using LR.Application.AppResult.ResultData.Photo;
using LR.Application.Interfaces.Services;
using LR.Application.Requests.User;
using LR.Application.Responses;
using LR.Application.Responses.UserProfile;
using LR.Infrastructure.Extensions;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LR.API.Controllers
{
    [Authorize]
    public class UserProfileController(
        UserManager<AppUser> userManager,
        IUserProfileService userProfileService,
        IValidator<ProfilePhotoUploadRequest> profilePhotoUploadValidator) : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IUserProfileService _userProfileService = userProfileService;
        private readonly IValidator<ProfilePhotoUploadRequest> _profilePhotoUploadValidator = profilePhotoUploadValidator;

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var appUser = await _userManager.GetByIdWithProfileAsync(User.GetAppUserId());

            if (appUser is null)
                return HandleFailure(UserProfileErrors.NotFound);

            var response = new MyProfileResponse
            {
                UserName = appUser.UserName!,
                FirstName = appUser.Profile.FirstName,
                LastName = appUser.Profile.LastName,
                Email = appUser.Email,
                ProfilePhotoUrl = appUser.Profile.ProfilePhotoUrl,
                IsEmailConfirmed = appUser.EmailConfirmed,
                BirthDate = appUser.Profile.BirthDate
            };

            return Ok(ApiResponse<MyProfileResponse>.Ok(response));
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
    }
}
