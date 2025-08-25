using LR.Application.DTOs.User;
using LR.Application.AppResult;

namespace LR.Application.Interfaces.Utils
{
    public interface IAccountService
    {
        Task<Result> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<TokenPairDto> LoginAsync(UserLoginDto userLoginDto);
        Task LogoutAsync(string userId);
        Task<TokenPairDto> RefreshToken(string? refreshToken);
    }
}
