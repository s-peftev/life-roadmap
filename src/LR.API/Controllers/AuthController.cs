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
using LR.Infrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace LR.API.Controllers
{
    public class AuthController(
        IAccountService accountService,
        IMapper mapper,
        IOptions<JwtOptions> jwtOptions,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
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
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly RefreshTokenOptions _refreshTokenOptions = refreshTokenOptions.Value;
        private readonly IValidator<UserRegisterRequest> _registerValidator = registerValidator;
        private readonly IValidator<UserLoginRequest> _loginValidator = loginValidator;
        private readonly IValidator<EmailCodeRequest> _emailCodeValidator = emailCodeValidator;
        private readonly IValidator<EmailConfirmationRequest> _emailConfirmationValidator = emailConfirmationValidator;
        private readonly IValidator<ForgotPasswordRequest> _forgotPasswordRequestValidator = forgotPasswordRequestValidator;
        private readonly IValidator<ResetPasswordRequest> _resetPasswordRequestValidator = resetPasswordRequestValidator;
        private readonly IValidator<ChangePasswordRequest> _changePasswordRequestValidator = changePasswordRequestValidator;
        private readonly IRefreshTokenCookieWriter _refreshTokenCookieWriter = refreshTokenCookieWriter;

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register new user",
            Description = "Creates a new user account and returns tokens")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<AccessTokenDto>),
            Description = "User registered successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>),
            Description = "Validation errors or registration failed")]
        [SwaggerResponse(StatusCodes.Status409Conflict, Type = typeof(ApiResponse<object>),
            Description = "User with given username or email already exists")]
        public async Task<IActionResult> Register(
            [FromBody] UserRegisterRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _registerValidator
                .ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidRegisterRequest
                    with { Details = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var registerResult = await _accountService
                .RegisterAsync(_mapper.Map<UserRegisterDto>(request), cancellationToken);

            return registerResult.Match(
                data =>
                {
                    _refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<AccessTokenDto>.Ok(data.AccessToken));
                },
                error => HandleFailure(error));
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login user", Description = "Authenticates and returns tokens")]
        [SwaggerResponse(StatusCodes.Status200OK, "Login successful", typeof(ApiResponse<AccessTokenDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation errors")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid username or password")]
        public async Task<IActionResult> Login(
            [FromBody] UserLoginRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _loginValidator
                .ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidLoginRequest
                    with { Details = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var loginResult = await _accountService
                .LoginAsync(_mapper.Map<UserLoginDto>(request), cancellationToken);

            return loginResult.Match(
                data =>
                {
                    _refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<AccessTokenDto>.Ok(data.AccessToken));
                },
                error => HandleFailure(error));
        }

        [HttpPost("refresh")]
        [SwaggerOperation(Summary = "Refresh jwt token", Description = "Generates new JWT token")]
        [SwaggerResponse(StatusCodes.Status200OK, "Tokens successfully updated", typeof(ApiResponse<AccessTokenDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Refresh token missing")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid or expired refresh token")]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
        {
            var refreshTokenValue = Request.Cookies[CookieNames.RefreshToken];

            if (string.IsNullOrEmpty(refreshTokenValue))
                return HandleFailure(RefreshTokenErrors.TokenMissing);

            var refreshResult = await _accountService
                .RefreshToken(refreshTokenValue, cancellationToken);
            
            return refreshResult.Match(
                data =>
                {
                    _refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<AccessTokenDto>.Ok(data.AccessToken));
                },
                error => 
                {
                    _refreshTokenCookieWriter.Delete();

                    return HandleFailure(error);
                }
            );
        }

        [HttpPost("logout")]
        [SwaggerOperation(Summary = "Logout user", Description = "Revokes refresh token and clears auth cookie")]
        [SwaggerResponse(StatusCodes.Status200OK, "Logout successful", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Refresh token missing")]
        public async Task<IActionResult> Logout()
        {
            var refreshTokenValue = Request.Cookies[CookieNames.RefreshToken];

            if(string.IsNullOrEmpty(refreshTokenValue))
                return HandleFailure(RefreshTokenErrors.TokenMissing);

            var result = await _accountService.LogoutAsync(refreshTokenValue);

            _refreshTokenCookieWriter.Delete();

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
        public async Task<IActionResult> SendEmailVerificationCode(
            [FromBody] EmailCodeRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _emailCodeValidator
                .ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidEmailCodeRequest
                    with
                { Details = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var codeResult = await _accountService.GenerateEmailConfirmationCodeAsync(request);

            return codeResult.Match(
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
        public async Task<IActionResult> ConfirmEmail(
            [FromBody] EmailConfirmationRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _emailConfirmationValidator
                .ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.EmailConfirmationFailed
                    with
                { Details = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var result = await _accountService.ConfirmEmailAsync(request);

            return result.Match(
                    () => Ok(ApiResponse<object>.Ok()),
                    error => HandleFailure(error)
                );
        }

        [Authorize]
        [HttpPatch("password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _changePasswordRequestValidator
                .ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidChangePasswordRequest
                    with
                { Details = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var result = await _accountService.ChangePasswordAsync(request, User.GetAppUserId());

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
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _forgotPasswordRequestValidator
                .ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidForgotPasswordRequest
                    with
                { Details = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var result = await _accountService.GeneratePasswordResetTokenAsync(request);

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
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordRequest request,
            CancellationToken cancellationToken)
        { 
            var validationResult = await _resetPasswordRequestValidator
                .ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            { 
                return HandleFailure(UserErrors.PasswordResetFailed
                    with
                { Details = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var result = await _accountService.ResetPasswordAsync(request);

            return result.Match(
                    () => Ok(ApiResponse<object>.Ok()),
                    error => HandleFailure(error)
                );
        }
    }
}
