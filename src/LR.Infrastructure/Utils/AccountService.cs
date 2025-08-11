using AutoMapper;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
using LR.Domain.Entities.Users;
using LR.Domain.Exceptions.User;
using LR.Infrastructure.Exceptions.Account;
using LR.Infrastructure.Extensions;
using LR.Infrastructure.Options;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LR.Infrastructure.Utils
{
    public class AccountService(
        ITokenService tokenService,
        IOptions<JwtOptions> jwtOptions,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        IMapper mapper,
        IUserProfileService userProfileService,
        IRefreshTokenService refreshTokenService,
        UserManager<AppUser> userManager
        ) : IAccountService
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly RefreshTokenOptions _refreshTokenOptions = refreshTokenOptions.Value;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IUserProfileService _userProfileService = userProfileService;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

        public async Task RegisterAsync(UserRegisterDto userRegisterDto)
        {
            if (await _userManager.FindByNameAsync(userRegisterDto.UserName) is not null)
                throw new UserAlreadyExistsException(
                    $"User with username '{userRegisterDto.UserName}' already exists.");

            if (!string.IsNullOrEmpty(userRegisterDto.Email) &&
                await _userManager.FindByEmailAsync(userRegisterDto.Email) is not null)
            {
                throw new UserAlreadyExistsException($"Email '{userRegisterDto.Email}' is already taken.");
            }

            var user = _mapper.Map<AppUser>(userRegisterDto);
            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (!result.Succeeded)
            {
                throw new RegistrationFailedException(userRegisterDto.UserName);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                throw new RegistrationFailedException(userRegisterDto.UserName);
            }

            var profile = _mapper.Map<UserProfile>(userRegisterDto);
            profile.UserId = user.Id;

            _userProfileService.Add(profile);
            try
            {
                await _userProfileService.SaveChangesAsync();
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);

                throw new RegistrationFailedException(userRegisterDto.UserName);
            }
        }

        public async Task LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _userManager.GetByUserNameWithRolesAsync(userLoginDto.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            {
                throw new LoginFailedException(userLoginDto.UserName);
            }

            await SaveTokensAndSetCookiesAsync(user);
        }

        public async Task RefreshToken(string? refreshTokenValue)
        {
            if (string.IsNullOrEmpty(refreshTokenValue))
            {
                throw new RefreshTokenException("Refresh token is missing.");
            }

            var refreshToken = await _refreshTokenService.GetByTokenValueAsync(refreshTokenValue) 
                ?? throw new RefreshTokenException("Could not find the refresh token.");

            var user = await _userManager.GetByIdWithRolesAsync(refreshToken.UserId)
                ?? throw new RefreshTokenException("Unable to retrieve user for refresh token.");

            if (refreshToken.IsRevoked)
            {
                throw new RefreshTokenException("Refresh token is revoked.");
            }

            if (refreshToken.ExpiresAtUtc < DateTime.UtcNow)
            {
                throw new RefreshTokenException("Refresh token is expired.");
            }

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAtUtc = DateTime.UtcNow;
            await _refreshTokenService.SaveChangesAsync();

            await SaveTokensAndSetCookiesAsync(user);
        }

        public async Task LogoutAsync(string? refreshTokenValue)
        {
            if (string.IsNullOrEmpty(refreshTokenValue))
            {
                throw new RefreshTokenException("Refresh token is missing.");
            }

            var refreshToken = await _refreshTokenService.GetByTokenValueAsync(refreshTokenValue)
                ?? throw new RefreshTokenException("Could not find the refresh token.");


            refreshToken.IsRevoked = true;
            refreshToken.RevokedAtUtc = DateTime.UtcNow;

            await _refreshTokenService.SaveChangesAsync();
        }

        private async Task SaveTokensAndSetCookiesAsync(AppUser user)
        {
            var (jwtToken, expirationDateInUtc) =
                _tokenService.GenerateJwtToken(_mapper.Map<TokenUserDto>(user));
            var refreshToken = _tokenService
                .GenerateRefreshToken(user.Id, _refreshTokenOptions.ExpirationTimeInDays);

            try
            {
                _refreshTokenService.Add(refreshToken);
                await _refreshTokenService.SaveChangesAsync();

                _tokenService.WriteAuthTokenAsHttpOnlyCookie(
                    _jwtOptions.TokenName,
                    jwtToken,
                    expirationDateInUtc);

                _tokenService.WriteAuthTokenAsHttpOnlyCookie(
                    _refreshTokenOptions.TokenName,
                    refreshToken.Token,
                    refreshToken.ExpiresAtUtc);
            }
            catch (Exception ex)
            {
                throw new AuthenticationTokenException("Token persistence failed", ex);
            }
        }
    }
}
