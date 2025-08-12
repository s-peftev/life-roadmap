using AutoMapper;
using FluentValidation;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Utils;
using LR.Application.Requests.User;
using LR.Application.Responses.User;
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
        IValidator<UserLoginRequest> loginValidator
        ) : BaseApiController
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly RefreshTokenOptions _refreshTokenOptions = refreshTokenOptions.Value;
        private readonly IValidator<UserRegisterRequest> _registerValidator = registerValidator;
        private readonly IValidator<UserLoginRequest> _loginValidator = loginValidator;

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register new user", Description = "Creates a new user account")]
        [SwaggerResponse(StatusCodes.Status200OK, "User registered successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation errors or registration failed")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "User with given username or email already exists")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _accountService.RegisterAsync(_mapper.Map<UserRegisterDto>(request));

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login user", Description = "Authenticates and returns tokens")]
        [SwaggerResponse(StatusCodes.Status200OK, "Login successful", typeof(AuthResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation errors")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid username or password")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] UserLoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var tokenPair = await _accountService.LoginAsync(_mapper.Map<UserLoginDto>(request));
            var response = _mapper.Map<AuthResponse>(tokenPair);
            response.Message = "Login successful";

            return Ok(response);
        }

        [HttpPost("refresh")]
        [SwaggerOperation(Summary = "Refresh tokens", Description = "Generates new JWT and Refresh token")]
        [SwaggerResponse(StatusCodes.Status200OK, "Tokens successfully updated", typeof(AuthResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Refresh token missing or invalid")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid or expired refresh token")]
        public async Task<ActionResult<AuthResponse>> Refresh()
        {
            var refreshToken = Request.Cookies[_refreshTokenOptions.TokenName];

            var tokenPair = await _accountService.RefreshToken(refreshToken);
            var response = _mapper.Map<AuthResponse>(tokenPair);
            response.Message = "Tokens successfully updated";

            return Ok(response);
        }

        [HttpPost("logout")]
        [SwaggerOperation(Summary = "Logout user", Description = "Revokes refresh token and clears auth cookies")]
        [SwaggerResponse(StatusCodes.Status200OK, "Logout successful")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Refresh token missing")]
        public async Task<IActionResult> Logout()
        {
            var refreshTokenVAlue = Request.Cookies[_refreshTokenOptions.TokenName];
            await _accountService.LogoutAsync(refreshTokenVAlue);

            Response.Cookies.Delete(_jwtOptions.TokenName);
            Response.Cookies.Delete(_refreshTokenOptions.TokenName);

            return Ok(new { message = "Logout successful" });
        }
    }
}
