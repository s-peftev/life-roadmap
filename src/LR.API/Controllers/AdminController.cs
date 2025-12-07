using LR.Application.AppResult.ResultData;
using LR.Application.DTOs.Admin;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
using LR.Application.Requests;
using LR.Application.Responses;
using LR.Infrastructure.Constants;
using LR.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = Policies.RequireAdministratorRole)]
    public class AdminController(
        IErrorResponseFactory errorResponseFactory,
        IAdminService adminService,
        IUserProfileService userProfileService)
        : Controller
    {
        [HttpGet("users")]
        [SwaggerOperation("Get list of users", "Returns all users visible to administrator")]

        [SwaggerResponse(StatusCodes.Status200OK, "List of users", typeof(ApiResponse<IEnumerable<UserForAdminDto>>))]
        public async Task<IActionResult> GetUsers(PaginatedRequest request, CancellationToken ct)
        { 
            var result = await adminService.GetUserListAsync(request, User.GetAppUserId(), ct);

            return result.Match(
                data => Ok(ApiResponse<PaginatedResult<UserForAdminDto>>.Ok(data)),
                error => errorResponseFactory.CreateErrorResponse(error)
            );
        }

        [HttpDelete("users/{userId}/photo")]
        [SwaggerOperation("Delete user`s profile photo", "Deletes the user's profile photo by user ID.")]

        [SwaggerResponse(StatusCodes.Status200OK,                  "User`s profile photo has been deleted.", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Failed to delete user`s profile photo.", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable,  "Service is unavailable.",              typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound,            "User`s profile not found.",            typeof(ApiResponse<object>))]
        public async Task<IActionResult> DeleteUserProfilePhoto(string userId, CancellationToken ct)
        {
            var result = await userProfileService.DeleteProfilePhotoAsync(userId, ct);

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => errorResponseFactory.CreateErrorResponse(error)
            );
        }
    }
}
