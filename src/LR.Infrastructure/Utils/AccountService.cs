using AutoMapper;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
using LR.Domain.Entities.Users;
using LR.Domain.Exceptions.User;
using LR.Infrastructure.Exceptions.Account;
using LR.Infrastructure.Extensions;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;

namespace LR.Infrastructure.Utils
{
    public class AccountService(
        ITokenService tokenService,
        UserManager<AppUser> userManager,
        IMapper mapper,
        IUserProfileService userProfileService
        ) : IAccountService
    {
        private const int RefreshTokenExpirationDays = 7;

        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IUserProfileService _userProfileService = userProfileService;

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
            var user = await _userManager.GetByUserNameWithProfileAndRolesAsync(userLoginDto.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            {
                throw new LoginFailedException(userLoginDto.UserName);
            }


            await SaveTokensAndSetCookiesAsync(user);
        }

        public async Task RefreshToken(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new RefreshTokenException("Refresh token is missing.");
            }

            var userProfile = await _userProfileService.GetByUserProfileByRefreshTokenAsync(refreshToken) 
                ?? throw new RefreshTokenException("Unable to retrieve user profile for refresh token.");

            var user = await _userManager.FindByIdAsync(userProfile.UserId)
                ?? throw new RefreshTokenException("Unable to retrieve user for refresh token."); ;

            if (userProfile.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
            {
                throw new RefreshTokenException("Refresh token is expired.");
            }

            await SaveTokensAndSetCookiesAsync(user);
        }

        public async Task LogoutAsync(string userId)
        {
            var user = await _userManager.GetByIdWithProfileAsync(userId)
                ?? throw new LogoutFailedException("Could not found user to logout.");

            user.Profile.RefreshToken = null;
            user.Profile.RefreshTokenExpiresAtUtc = null;

            await _userProfileService.SaveChangesAsync();
        }

        private async Task SaveTokensAndSetCookiesAsync(AppUser user)
        {
            var (jwtToken, expirationDateInUtc) =
                _tokenService.GenerateJwtToken(_mapper.Map<TokenUserDto>(user));
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays);

            user.Profile.RefreshToken = refreshToken;
            user.Profile.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

            try
            {
                await _userProfileService.SaveChangesAsync();

                _tokenService
                    .WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
                _tokenService
                    .WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", refreshToken, refreshTokenExpirationDateInUtc);

            }
            catch (Exception ex)
            {
                throw new AuthenticationTokenException("Token persistence failed", ex);
            }
        }
    }
}
