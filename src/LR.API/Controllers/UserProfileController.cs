using LR.Application.Interfaces.ExternalProviders;
using LR.Application.Interfaces.Services;
using LR.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LR.API.Controllers
{
    [Authorize]
    public class UserProfileController(
        IPhotoService photoService,
        IUserProfileService userProfileService) : BaseApiController
    {
        private readonly IPhotoService _photoService = photoService;
        private readonly IUserProfileService _userProfileService = userProfileService;

        [HttpPost("photo/upload")]
        public async Task<IActionResult> UploadPhoto([FromForm]IFormFile file, CancellationToken cancellationToken)
        {
            var userProfile = _userProfileService.GetByUserIdAsync(User.GetAppUserId(), cancellationToken);
        }
    }
}
