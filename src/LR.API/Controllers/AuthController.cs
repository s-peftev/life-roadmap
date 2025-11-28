using AutoMapper;
using LR.Application.AppResult.Errors.User;
using LR.Application.DTOs.Token;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Utils;
using LR.Application.Requests.User;
using LR.Application.Responses;
using LR.Infrastructure.Constants;
using LR.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LR.API.Controllers
{
    public class AuthController(
        IErrorResponseFactory errorResponseFactory,
        IAccountService accountService,
        IMapper mapper,
        IRefreshTokenCookieWriter refreshTokenCookieWriter
        ) : BaseApiController
    {
        [HttpPost("register")]
        [SwaggerOperation("Register new user", "Creates a new user account and returns tokens")]

        [SwaggerResponse(StatusCodes.Status200OK,         "User has been registered",                            typeof(ApiResponse<AccessTokenDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation errors or registration failed",            typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status409Conflict,   "User with given username or email is already exists", typeof(ApiResponse<object>))]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request, CancellationToken ct)
        {
            var result = await accountService.RegisterAsync(mapper.Map<UserRegisterDto>(request), ct);

            return result.Match(
                data =>
                {
                    refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<AccessTokenDto>.Ok(data.AccessToken));
                },
                error => errorResponseFactory.CreateErrorResponse(error)
            );
        }

        [HttpPost("login")]
        [SwaggerOperation("Login user", "Authenticates and returns tokens")]

        [SwaggerResponse(StatusCodes.Status200OK,           "Login successful",             typeof(ApiResponse<AccessTokenDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,   "Validation errors",            typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid username or password", typeof(ApiResponse<object>))]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request, CancellationToken ct)
        {
            var result = await accountService.LoginAsync(mapper.Map<UserLoginDto>(request), ct);

            return result.Match(
                data =>
                {
                    refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<AccessTokenDto>.Ok(data.AccessToken));
                },
                error => errorResponseFactory.CreateErrorResponse(error)
            );
        }

        [HttpPost("refresh")]
        [SwaggerOperation("Refresh user`s tokens", "Refreshes the JWT token and rotates refresh token as needed")]

        [SwaggerResponse(StatusCodes.Status200OK,           "Tokens has been updated",             typeof(ApiResponse<AccessTokenDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,   "Refresh token is missing",            typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid or expired refresh token",    typeof(ApiResponse<object>))]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            var refreshTokenValue = Request.Cookies[CookieNames.RefreshToken];

            if (string.IsNullOrEmpty(refreshTokenValue))
                return errorResponseFactory.CreateErrorResponse(RefreshTokenErrors.TokenMissing);

            var result = await accountService.RefreshToken(refreshTokenValue, ct);
            
            return result.Match(
                data =>
                {
                    refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<AccessTokenDto>.Ok(data.AccessToken));
                },
                error => 
                {
                    refreshTokenCookieWriter.Delete();

                    return errorResponseFactory.CreateErrorResponse(error);
                }
            );
        }

        [HttpPost("logout")]
        [SwaggerOperation("Logout user", "Revokes refresh token and clears auth cookie")]

        [SwaggerResponse(StatusCodes.Status200OK,         "Logout successful",        typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Refresh token is missing", typeof(ApiResponse<object>))]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var refreshTokenValue = Request.Cookies[CookieNames.RefreshToken];

            if(string.IsNullOrEmpty(refreshTokenValue))
                return errorResponseFactory.CreateErrorResponse(RefreshTokenErrors.TokenMissing);

            var result = await accountService.LogoutAsync(refreshTokenValue, ct);

            refreshTokenCookieWriter.Delete();

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => errorResponseFactory.CreateErrorResponse(error)
            );
        }

        [HttpPost("email/verification-code")]
        [SwaggerOperation("Send email verification code", "Generates and sends a new email verification code for user confirmation.")]

        [SwaggerResponse(StatusCodes.Status200OK,         "Verification code has been generated",           typeof(ApiResponse<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request for email confirmation code is invalid", typeof(ApiResponse<string>))]
        public async Task<IActionResult> SendEmailVerificationCode([FromBody] EmailCodeRequest request)
        {
            var result = await accountService.GenerateEmailConfirmationCodeAsync(request);

            return result.Match(
                data => Ok(ApiResponse<string>.Ok(data)),
                error => errorResponseFactory.CreateErrorResponse(error)
            ); 
        }

        [HttpPost("email/verification")]
        [SwaggerOperation("Confirm user email", "Confirms user's email using the verification code.")]

        [SwaggerResponse(StatusCodes.Status200OK,         "Email has been confirmed",     typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Email confirmation failed",    typeof(ApiResponse<object>))]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationRequest request)
        {
            var result = await accountService.ConfirmEmailAsync(request);

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => errorResponseFactory.CreateErrorResponse(error)
            );
        }

        [Authorize]
        [HttpPatch("password")]
        [SwaggerOperation("Reset user`s password", "Sets a new password for authenticated user if correct current password is provided.")]

        [SwaggerResponse(StatusCodes.Status200OK,         "Password has been changed.",                                            typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Reset password request is invalid or wrong current password provided.", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound,   "User not found.",                                                       typeof(ApiResponse<object>))]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await accountService.ChangePasswordAsync(request, User.GetAppUserId());

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => errorResponseFactory.CreateErrorResponse(error)
            );
        }

        [HttpPost("password/reset-request")]
        [SwaggerOperation("Send password reset token", "Generates and sends a new password reset token for user confirmation.")]

        [SwaggerResponse(StatusCodes.Status200OK,         "Password reset token generated",              typeof(ApiResponse<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request for password reset token is invalid", typeof(ApiResponse<string>))]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await accountService.GeneratePasswordResetTokenAsync(request);

            return result.Match(
                data => Ok(ApiResponse<string>.Ok(data)),
                error => errorResponseFactory.CreateErrorResponse(error)
            );
        }

        [HttpPost("password/reset")]
        [SwaggerOperation("Reset user password", "Resets user's password using the reset token.")]

        [SwaggerResponse(StatusCodes.Status200OK,         "Password has been reset",         typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Password reset failed",           typeof(ApiResponse<object>))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        { 
            var result = await accountService.ResetPasswordAsync(request);

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => errorResponseFactory.CreateErrorResponse(error)
            );
        }
    }
}
