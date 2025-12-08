using LR.Application.AppResult;
using LR.Application.AppResult.ResultData;
using LR.Application.DTOs.Admin;
using LR.Application.DTOs.User;
using LR.Application.Requests;

namespace LR.Application.Interfaces.Utils
{
    public interface IAppUserService
    {
        Task<Result<UserProfileDetailsDto>> GetProfileDetailsAsync(string userId, CancellationToken ct = default);
        Task<Result<PaginatedResult<UserForAdminDto>>> GetUsersForAdminAsync(PaginatedRequest request, string adminId, CancellationToken ct = default);
    }
}