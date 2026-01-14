using LR.Application.AppResult;
using LR.Application.AppResult.ResultData;
using LR.Application.DTOs.Admin;
using LR.Application.DTOs.User;
using LR.Application.Requests.Admin;

namespace LR.Application.Interfaces.Utils
{
    public interface IAppUserService
    {
        Task<Result<UserProfileDetailsDto>> GetProfileDetailsAsync(string userId, CancellationToken ct = default);
        Task<Result<PaginatedResult<UserForAdminDto>>> GetUsersForAdminAsync(UsersForAdminRequest request, string adminId, CancellationToken ct = default);
    }
}