using LR.Application.DTOs.User;

namespace LR.Application.Interfaces.Utils
{
    public interface IAccountService
    {
        Task RegisterAsync(UserRegisterDto userRegisterDto);
        Task<TokenPairDto> LoginAsync(UserLoginDto userLoginDto);
        Task LogoutAsync(string userId);
        Task<TokenPairDto> RefreshToken(string? refreshToken);
    }
}
