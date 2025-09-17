using LR.Application.DTOs.Admin;
using LR.Application.Interfaces.Utils;
using LR.Application.Responses;
using LR.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LR.API.Controllers
{
    [Authorize(Policy = Policies.RequireAdministratorRole)]
    public class AdminController(
        IAdminService adminService) : BaseApiController
    {
        private readonly IAdminService _adminService = adminService;

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        { 
            var userListResult = await _adminService.GetUserListAsync(cancellationToken);

            return userListResult.Match(
                data => Ok(ApiResponse<IEnumerable<UserForAdminDto>>.Ok(data)),
                error => HandleFailure(error)
                );
        }
    }
}
