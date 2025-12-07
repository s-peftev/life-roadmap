using LR.Application.AppResult;
using LR.Application.AppResult.ResultData;
using LR.Application.DTOs.Admin;
using LR.Application.Requests;

namespace LR.Application.Interfaces.Utils
{
    public interface IAdminService
    {
        Task<Result<PaginatedResult<UserForAdminDto>>> GetUserListAsync(PaginatedRequest request, string adminId, CancellationToken ct = default);
    }
}