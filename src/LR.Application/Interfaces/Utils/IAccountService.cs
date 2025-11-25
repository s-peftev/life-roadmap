using LR.Application.DTOs.User;
using LR.Application.AppResult;
using LR.Application.AppResult.ResultData.Account;
using LR.Application.Requests.User;

namespace LR.Application.Interfaces.Utils
{
    public interface IAccountService
    {
        Task<Result<AuthResult>> RegisterAsync(
            UserRegisterDto userRegisterDto,
            CancellationToken ct = default);
        Task<Result<AuthResult>> LoginAsync(
            UserLoginDto userLoginDto,
            CancellationToken ct = default);
        Task<Result> LogoutAsync(string userId);
        Task<Result<AuthResult>> RefreshToken(
            string refreshToken,
            CancellationToken ct = default);
        Task<Result<string>> GenerateEmailConfirmationCodeAsync(
            EmailCodeRequest emailCodeRequest);
        Task<Result> ConfirmEmailAsync(
            EmailConfirmationRequest emailConfirmationRequest);
        Task<Result<string>> GeneratePasswordResetTokenAsync(
            ForgotPasswordRequest forgotPasswordRequest);
        Task<Result> ResetPasswordAsync(
            ResetPasswordRequest resetPasswordRequest);
        Task<Result> ChangeUsernameAsync(
            ChangeUsernameRequest changeUsernameRequest,
            string userId);
        Task<Result> ChangePasswordAsync(
            ChangePasswordRequest changePasswordRequest,
            string userId);
    }
}
