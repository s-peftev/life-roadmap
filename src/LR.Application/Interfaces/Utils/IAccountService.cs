using LR.Application.DTOs.User;
using LR.Application.AppResult;
using LR.Application.Responses.User;
using LR.Application.DTOs.Token;

namespace LR.Application.Interfaces.Utils
{
    public interface IAccountService
    {
        Task<Result<AuthResponse>> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<TokenPairDto> LoginAsync(UserLoginDto userLoginDto);
        Task LogoutAsync(string userId);
        Task<TokenPairDto> RefreshToken(string? refreshToken);
    }
}
