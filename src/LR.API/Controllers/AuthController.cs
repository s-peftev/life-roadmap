using AutoMapper;
using FluentValidation;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Utils;
using LR.Application.Requests;
using LR.Infrastructure.Extensions;
using LR.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _accountService.LoginAsync(_mapper.Map<UserLoginDto>(request));

            return Ok(new { message = "Login successful" });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies[_refreshTokenOptions.TokenName];
            await _accountService.RefreshToken(refreshToken);

            return Ok(new { message = "Tokens successfully updated" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies[_refreshTokenOptions.TokenName];
            await _accountService.LogoutAsync(refreshToken);

            Response.Cookies.Delete(_jwtOptions.TokenName);
            Response.Cookies.Delete(_refreshTokenOptions.TokenName);

            return Ok(new { message = "Logout successful" });
        }
    }
}
