using AutoMapper;
using FluentValidation;
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
        IAccountService accountService,
        IMapper mapper,
        IValidator<UserRegisterRequest> registerValidator,
        IValidator<UserLoginRequest> loginValidator,
        IValidator<EmailCodeRequest> emailCodeValidator,
        IValidator<EmailConfirmationRequest> emailConfirmationValidator,
        IValidator<ForgotPasswordRequest> forgotPasswordRequestValidator,
        IValidator<ResetPasswordRequest> resetPasswordRequestValidator,
        IValidator<ChangePasswordRequest> changePasswordRequestValidator,
        IRefreshTokenCookieWriter refreshTokenCookieWriter
        ) : BaseApiController
    {
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register new user",
            Description = "Creates a new user account and returns tokens")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<AccessTokenDto>),
            Description = "User registered successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>),
            Description = "Validation errors or registration failed")]
        [SwaggerResponse(StatusCodes.Status409Conflict, Type = typeof(ApiResponse<object>),
            Description = "User with given username or email already exists")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request, CancellationToken ct)
        {
            var validationResult = await registerValidator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidRegisterRequest with 
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage) 
                });
            }

            var result = await accountService.RegisterAsync(mapper.Map<UserRegisterDto>(request), ct);

            return result.Match(
                data =>
                {
                    refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<AccessTokenDto>.Ok(data.AccessToken));
                },
                error => HandleFailure(error)
            );
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login user", Description = "Authenticates and returns tokens")]
        [SwaggerResponse(StatusCodes.Status200OK, "Login successful", typeof(ApiResponse<AccessTokenDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation errors")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid username or password")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request, CancellationToken ct)
        {
            var validationResult = await loginValidator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidLoginRequest with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await accountService.LoginAsync(mapper.Map<UserLoginDto>(request), ct);

            return result.Match(
                data =>
                {
                    refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<AccessTokenDto>.Ok(data.AccessToken));
                },
                error => HandleFailure(error)
            );
        }

        [HttpPost("refresh")]
        [SwaggerOperation(Summary = "Refresh jwt token", Description = "Generates new JWT token")]
        [SwaggerResponse(StatusCodes.Status200OK, "Tokens successfully updated", typeof(ApiResponse<AccessTokenDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Refresh token missing")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid or expired refresh token")]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            var refreshTokenValue = Request.Cookies[CookieNames.RefreshToken];

            if (string.IsNullOrEmpty(refreshTokenValue))
                return HandleFailure(RefreshTokenErrors.TokenMissing);

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

                    return HandleFailure(error);
                }
            );
        }

        [HttpPost("logout")]
        [SwaggerOperation(Summary = "Logout user", Description = "Revokes refresh token and clears auth cookie")]
        [SwaggerResponse(StatusCodes.Status200OK, "Logout successful", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Refresh token missing")]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var refreshTokenValue = Request.Cookies[CookieNames.RefreshToken];

            if(string.IsNullOrEmpty(refreshTokenValue))
                return HandleFailure(RefreshTokenErrors.TokenMissing);

            var result = await accountService.LogoutAsync(refreshTokenValue, ct);

            refreshTokenCookieWriter.Delete();

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }

        [HttpPost("email/verification-code")]
        [SwaggerOperation(
            Summary = "Send email verification code",
            Description = "Generates and sends a new email verification code for user confirmation.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Verification code successfully generated", typeof(ApiResponse<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request for email confirmation code is invalid", typeof(ApiResponse<string>))]
        public async Task<IActionResult> SendEmailVerificationCode([FromBody] EmailCodeRequest request, CancellationToken ct)
        {
            var validationResult = await emailCodeValidator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidEmailCodeRequest with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await accountService.GenerateEmailConfirmationCodeAsync(request);

            return result.Match(
                data => Ok(ApiResponse<string>.Ok(data)),
                error => HandleFailure(error)
            ); 
        }

        [HttpPost("email/verification")]
        [SwaggerOperation(
            Summary = "Confirm user email",
            Description = "Confirms user's email using the verification code.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Email successfully confirmed", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Email confirmation failed", typeof(ApiResponse<object>))]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationRequest request, CancellationToken ct)
        {
            var validationResult = await emailConfirmationValidator.ValidateAsync(request, ct);
            
            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.EmailConfirmationFailed with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await accountService.ConfirmEmailAsync(request);

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }

        [Authorize]
        [HttpPatch("password")]
        [SwaggerOperation(
            Summary = "Changes users` password",
            Description = "Sets a new password for authenticated user if correct current password is provided.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Password was successfully changed.", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, 
            "Request for password change is invalid or wrong current password provided.", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.", typeof(ApiResponse<object>))]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken ct)
        {
            var validationResult = await changePasswordRequestValidator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidChangePasswordRequest with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await accountService.ChangePasswordAsync(request, User.GetAppUserId());

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }

        [HttpPost("password/reset-request")]
        [SwaggerOperation(
            Summary = "Send password reset token",
            Description = "Generates and sends a new password reset token for user confirmation.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Password reset token successfully generated", typeof(ApiResponse<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request for password reset token is invalid", typeof(ApiResponse<string>))]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
        {
            var validationResult = await forgotPasswordRequestValidator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidForgotPasswordRequest with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await accountService.GeneratePasswordResetTokenAsync(request);

            return result.Match(
                data => Ok(ApiResponse<string>.Ok(data)),
                error => HandleFailure(error)
            );
        }

        [HttpPost("password/reset")]
        [SwaggerOperation(
            Summary = "Reset user password",
            Description = "Resets user's password using the reset token.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Password was successfully reset", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Password reset failed", typeof(ApiResponse<object>))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
        { 
            var validationResult = await resetPasswordRequestValidator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            { 
                return HandleFailure(UserErrors.PasswordResetFailed with
                {
                    Details = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await accountService.ResetPasswordAsync(request);

            return result.Match(
                () => Ok(ApiResponse<object>.Ok()),
                error => HandleFailure(error)
            );
        }
    }
}
