using LR.Application.DTOs.User;
using LR.Application.AppResult;
using LR.Application.DTOs.Token;
using LR.Application.AppResult.ResultData.Account;

namespace LR.Application.Interfaces.Utils
{
    public interface IAccountService
    {
        Task<Result<RegisterResultData>> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<TokenPairDto> LoginAsync(UserLoginDto userLoginDto);
        Task LogoutAsync(string userId);
        Task<TokenPairDto> RefreshToken(string? refreshToken);
    }
}
