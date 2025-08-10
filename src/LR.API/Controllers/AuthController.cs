using AutoMapper;
using FluentValidation;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Utils;
using LR.Application.Requests;
using LR.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LR.API.Controllers
{
    public class AuthController(
        IAccountService accountService,
        IMapper mapper,
        IValidator<UserRegisterRequest> registerValidator,
        IValidator<UserLoginRequest> loginValidator
        ) : BaseApiController
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;
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
            var refreshToken = Request.Cookies["REFRESH_TOKEN"];
            await _accountService.RefreshToken(refreshToken);

            return Ok(new { message = "Tokens successfully updated" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        { 
            var userId = User.GetUserId();
            await _accountService.LogoutAsync(userId);

            Response.Cookies.Delete("ACCESS_TOKEN");
            Response.Cookies.Delete("REFRESH_TOKEN");

            return Ok(new { message = "Logout successful" });
        }
    }
}
