using LR.Application.DTOs.Admin;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
using LR.Application.Responses;
using LR.Infrastructure.Constants;
using LR.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LR.API.Controllers
{
    [Authorize(Policy = Policies.RequireAdministratorRole)]
    public class AdminController(
        IAdminService adminService,
        IUserProfileService userProfileService) : BaseApiController
    {
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(CancellationToken ct)
        { 
            var userListResult = await adminService.GetUserListAsync(ct);

            return userListResult.Match(
                data => Ok(
                    ApiResponse<IEnumerable<UserForAdminDto>>.Ok(
                        data.Where(u => u.Id != User.GetAppUserId())
                    )
                ),
                error => HandleFailure(error)
            );
        }

        [HttpDelete("users/{userId}/photo")]
        public async Task<IActionResult> DeleteUserProfilePhoto(
            string userId,
            CancellationToken ct)
        {
            var deletionResult = await userProfileService
                .DeleteProfilePhotoAsync(userId, ct);

            return deletionResult.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }
    }
}
