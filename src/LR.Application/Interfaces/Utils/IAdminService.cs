using LR.Application.AppResult;
using LR.Application.DTOs.Admin;

namespace LR.Application.Interfaces.Utils
{
    public interface IAdminService
    {
        Task<Result<IEnumerable<UserForAdminDto>>> GetUserListAsync(string adminId, CancellationToken ct = default);
    }
}