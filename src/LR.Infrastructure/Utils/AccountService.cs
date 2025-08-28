using AutoMapper;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Domain.Entities.Users;
using LR.Domain.Enums;
using LR.Infrastructure.Exceptions.Account;
using LR.Infrastructure.Extensions;
using LR.Infrastructure.Options;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using LR.Application.DTOs.Token;
using LR.Application.AppResult.ResultData.Account;
using LR.Application.Responses.User;

namespace LR.Infrastructure.Utils
{
    public class AccountService(
        ITokenService tokenService,
        IOptions<JwtOptions> jwtOptions,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        IMapper mapper,
        IUserProfileService userProfileService,
        IRefreshTokenService refreshTokenService,
        IRequestInfoService requestInfoService,
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
        private readonly IRequestInfoService _requestInfoService = requestInfoService;

        public async Task<Result<AuthResult>> RegisterAsync(UserRegisterDto dto)
        {
            var userCheck = await EnsureUserIsUniqueAsync(dto);
            if (!userCheck.IsSuccess)
                return Result<AuthResult>.Failure(userCheck.Error);

            var userResult = await CreateUserAsync(dto);
            if (!userResult.IsSuccess)
                return Result<AuthResult>.Failure(userResult.Error);

            var user = userResult.Value;

            var roleAssignResult = await AssignDefaultRoleAsync(user);
            if (!roleAssignResult.IsSuccess)
            {
                await _userManager.DeleteAsync(user);
                return Result<AuthResult>.Failure(roleAssignResult.Error);
            }

            var profileResult = await CreateProfileAsync(user, dto);
            if (!profileResult.IsSuccess)
            {
                await _userManager.DeleteAsync(user);
                return Result<AuthResult>.Failure(profileResult.Error);
            }

            return await BuildAuthResultAsync(user);
        }

        public async Task<Result<AuthResult>> LoginAsync(UserLoginDto dto)
        {
            var user = await _userManager.GetByUserNameWithRolesAsync(dto.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return Result<AuthResult>.Failure(UserErrors.LoginFailed);
            }

            return await BuildAuthResultAsync(user);
        }

        public async Task<TokenPairResponse> RefreshToken(string? refreshTokenValue)
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

            return await SaveTokensAndSetCookiesAsync(user);
        }

        public async Task<Result> LogoutAsync(string refreshTokenValue)
        {
            var rtResult = await _refreshTokenService.GetByTokenValueAsync(refreshTokenValue);

            if (!rtResult.IsSuccess)
                return Result.Success();

            rtResult.Value.IsRevoked = true;
            rtResult.Value.RevokedAtUtc = DateTime.UtcNow;

            var saveResult = await _refreshTokenService.SaveChangesAsync();

            if (!saveResult.IsSuccess)
                return Result.Failure(RefreshTokenErrors.TokenRevokingFailed);

            return Result.Success();
        }

        private async Task<Result> EnsureUserIsUniqueAsync(UserRegisterDto dto)
        {
            if (await _userManager.FindByNameAsync(dto.UserName) is not null)
                return Result.Failure(UserErrors.UsernameIsTaken);

            if (!string.IsNullOrEmpty(dto.Email) &&
                await _userManager.FindByEmailAsync(dto.Email) is not null)
                return Result.Failure(UserErrors.EmailIsTaken);

            return Result.Success();
        }

        private async Task<Result<AppUser>> CreateUserAsync(UserRegisterDto dto)
        {
            var user = _mapper.Map<AppUser>(dto);
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var details = result.Errors.Select(e => e.Description);

                return Result<AppUser>.Failure(UserErrors.RegistrationFailed with { Details = details });
            }

            return Result<AppUser>.Success(user);
        }

        private async Task<Result> AssignDefaultRoleAsync(AppUser user)
        {
            var result = await _userManager.AddToRoleAsync(user, Role.User.ToString());
            
            if (!result.Succeeded)
                return Result.Failure(UserErrors.RegistrationFailed);

            return Result.Success();
        }

        private async Task<Result<UserProfile>> CreateProfileAsync(
            AppUser user,
            UserRegisterDto dto)
        {
            var profile = _mapper.Map<UserProfile>(dto);
            profile.UserId = user.Id;
            _userProfileService.Add(profile);

            var saveResult = await _userProfileService.SaveChangesAsync();

            if (saveResult.Value is 0)
                return Result<UserProfile>.Failure(UserErrors.RegistrationFailed);

            return Result<UserProfile>.Success(profile);
        }

        private async Task<Result<AuthResult>> BuildAuthResultAsync(AppUser user)
        {
            var jwtToken = _tokenService
                .GenerateJwtToken(_mapper.Map<TokenUserDto>(user));

            var refreshTokenGenerationDto = new RefreshTokenGenerationDto
            {
                UserId = user.Id,
                ExpirationDays = _refreshTokenOptions.ExpirationTimeInDays,
                SessionId = Guid.NewGuid(),
                UserAgent = _requestInfoService.GetUserAgent(),
                IpAddress = _requestInfoService.GetIpAddress()
            };

            var refreshToken = _tokenService.GenerateRefreshToken(refreshTokenGenerationDto);

            _refreshTokenService.Add(refreshToken);
            var saveResult = await _refreshTokenService.SaveChangesAsync();

            if (saveResult.Value is 0)
            {
                return Result<AuthResult>.Failure(RefreshTokenErrors.SaveFailed);
            }

            var authResponse = new AuthResponse
            {
                AccessToken = jwtToken,
                User = _mapper.Map<UserDto>(user)
            };

            var authResult = new AuthResult
            {
                AuthResponse = authResponse,
                RefreshToken = refreshToken
            };

            return Result<AuthResult>.Success(authResult);
        }
    }
}
