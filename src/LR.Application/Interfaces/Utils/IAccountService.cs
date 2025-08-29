using LR.Application.DTOs.User;
using LR.Application.AppResult;
using LR.Application.AppResult.ResultData.Account;

namespace LR.Application.Interfaces.Utils
{
    public interface IAccountService
    {
        Task<Result<AuthResult>> RegisterAsync(UserRegisterDto dto);
        Task<Result<AuthResult>> LoginAsync(UserLoginDto dto);
        Task<Result> LogoutAsync(string userId);
        Task<Result<AuthResult>> RefreshToken(string refreshToken);
    }
}
