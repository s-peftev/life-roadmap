using AutoMapper;
using FluentValidation;
using LR.Application.AppResult.Errors;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Utils;
using LR.Application.Requests.User;
using LR.Application.Responses;
using LR.Application.Responses.User;
using LR.Infrastructure.Constants;
using LR.Infrastructure.Options;
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
        IRefreshTokenCookieWriter refreshTokenCookieWriter
        ) : BaseApiController
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly RefreshTokenOptions _refreshTokenOptions = refreshTokenOptions.Value;
        private readonly IValidator<UserRegisterRequest> _registerValidator = registerValidator;
        private readonly IValidator<UserLoginRequest> _loginValidator = loginValidator;
        private readonly IRefreshTokenCookieWriter _refreshTokenCookieWriter = refreshTokenCookieWriter;

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register new user",
            Description = "Creates a new user account and returns tokens")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<AuthResponse>),
            Description = "User registered successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>),
            Description = "Validation errors or registration failed")]
        [SwaggerResponse(StatusCodes.Status409Conflict, Type = typeof(ApiResponse<object>),
            Description = "User with given username or email already exists")]
        public async Task<IActionResult> Register(
            [FromBody] UserRegisterRequest request,
            CancellationToken ct)
        {
            var validationResult = await _registerValidator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidRegisterRequest
                    with { Details = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var registerResult = await _accountService
                .RegisterAsync(_mapper.Map<UserRegisterDto>(request));

            return registerResult.Match(
                data =>
                {
                    _refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<AuthResponse>.Ok(data.AuthResponse));
                },
                error => HandleFailure(error));
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login user", Description = "Authenticates and returns tokens")]
        [SwaggerResponse(StatusCodes.Status200OK, "Login successful", typeof(ApiResponse<AuthResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation errors")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid username or password")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return HandleFailure(UserErrors.InvalidLoginRequest
                    with { Details = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var loginResult = await _accountService
                .LoginAsync(_mapper.Map<UserLoginDto>(request));

            return loginResult.Match(
                data =>
                {
                    _refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<AuthResponse>.Ok(data.AuthResponse));
                },
                error => HandleFailure(error));
        }

        [HttpPost("refresh")]
        [SwaggerOperation(Summary = "Refresh tokens", Description = "Generates new JWT and Refresh token")]
        [SwaggerResponse(StatusCodes.Status200OK, "Tokens successfully updated", typeof(ApiResponse<TokenPairResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Refresh token missing")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid or expired refresh token")]
        public async Task<IActionResult> Refresh()
        {
            var refreshTokenValue = Request.Cookies[CookieNames.RefreshToken];

            if (string.IsNullOrEmpty(refreshTokenValue))
                return HandleFailure(RefreshTokenErrors.TokenMissing);

            var tokenPairResult = await _accountService.RefreshToken(refreshTokenValue);
            
            return tokenPairResult.Match(
                data =>
                {
                    _refreshTokenCookieWriter.Set(data.RefreshToken.Token, data.RefreshToken.ExpiresAtUtc);

                    return Ok(ApiResponse<TokenPairResponse>.Ok(data));
                },
                error => HandleFailure(error)
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
                    () =>
                    {
                        _refreshTokenCookieWriter.Delete();

                        return Ok(ApiResponse<object>.Ok());
                    },
                    error => HandleFailure(error)
                );
        }
    }
}
