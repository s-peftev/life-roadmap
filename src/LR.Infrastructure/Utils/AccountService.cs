using AutoMapper;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Domain.Entities.Users;
using LR.Domain.Enums;
using LR.Infrastructure.Exceptions.RefreshToken;
using LR.Infrastructure.Extensions;
using LR.Infrastructure.Options;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using LR.Application.DTOs.Token;
using LR.Application.AppResult.ResultData.Account;
using LR.Application.Responses.User;
using LR.Infrastructure.Exceptions.User;

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

            await _userProfileService.BeginTransactionAsync();

            var userResult = await CreateUserAsync(dto);
            var user = userResult.Value;

            await AssignDefaultRoleAsync(user);
            await CreateProfileAsync(user, dto);

            await _userProfileService.CommitTransactionAsync();

            return await AuthenticateUserAsync(user);
        }

        public async Task<Result<AuthResult>> LoginAsync(UserLoginDto dto)
        {
            var user = await _userManager.GetByUserNameWithRolesAsync(dto.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Result<AuthResult>.Failure(UserErrors.LoginFailed);

            return await AuthenticateUserAsync(user);
        }

        public async Task<Result<AuthResult>> RefreshToken(string refreshTokenValue)
        {
            var rtResult = await _refreshTokenService.GetByTokenValueAsync(refreshTokenValue);
            if (!rtResult.IsSuccess)
                return Result<AuthResult>.Failure(RefreshTokenErrors.RefreshTokenInvalid);

            var refreshToken = rtResult.Value;
            var user = await _userManager.GetByIdWithRolesAsync(refreshToken.UserId);

            if (user is null ||
                refreshToken.IsRevoked ||
                (refreshToken.ExpiresAtUtc < DateTime.UtcNow))
                return Result<AuthResult>.Failure(RefreshTokenErrors.RefreshTokenInvalid);

            if (IsTimeToRotateToken(refreshToken))
            {
                refreshToken.IsRevoked = true;
                refreshToken.RevokedAtUtc = DateTime.UtcNow;
                var saveResult = await _refreshTokenService.SaveChangesAsync();

                if (!saveResult.IsSuccess)
                    throw new TokenRevokingException();

                return await AuthenticateUserAsync(user, refreshToken.SessionId);
            }

            var jwtToken = _tokenService
                .GenerateJwtToken(_mapper.Map<JwtGenerationDto>(user));

            return Result<AuthResult>.Success(BuildAuthResult(user, jwtToken, refreshToken));
        }

        public async Task<Result> LogoutAsync(string refreshTokenValue)
        {
            var rtResult = await _refreshTokenService.GetByTokenValueAsync(refreshTokenValue);

            if (!rtResult.IsSuccess)
                return Result.Success();

            rtResult.Value.IsRevoked = true;
            rtResult.Value.RevokedAtUtc = DateTime.UtcNow;

            await _refreshTokenService.SaveChangesAsync();

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
                throw new UserRegisterException();

            return Result<AppUser>.Success(user);
        }

        private async Task<Result> AssignDefaultRoleAsync(AppUser user)
        {
            var result = await _userManager.AddToRoleAsync(user, Role.User.ToString());
            
            if (!result.Succeeded)
                throw new UserRegisterException();

            return Result.Success();
        }

        private async Task<Result<UserProfile>> CreateProfileAsync(
            AppUser user, UserRegisterDto dto)
        {
            var profile = _mapper.Map<UserProfile>(dto);
            profile.UserId = user.Id;
            _userProfileService.Add(profile);

            var saveResult = await _userProfileService.SaveChangesAsync();

            if (saveResult.Value is 0)
                throw new UserRegisterException();

            return Result<UserProfile>.Success(profile);
        }

        private async Task<Result<AuthResult>> AuthenticateUserAsync(AppUser user, Guid? sessionId = default)
        {
            var jwtToken = _tokenService
                .GenerateJwtToken(_mapper.Map<JwtGenerationDto>(user));

            var refreshTokenGenerationDto = new RefreshTokenGenerationDto
            {
                UserId = user.Id,
                ExpirationDays = _refreshTokenOptions.ExpirationTimeInDays,
                SessionId = sessionId ?? Guid.NewGuid(),
                UserAgent = _requestInfoService.GetUserAgent(),
                IpAddress = _requestInfoService.GetIpAddress()
            };

            var refreshToken = _tokenService.GenerateRefreshToken(refreshTokenGenerationDto);

            _refreshTokenService.Add(refreshToken);
            var saveResult = await _refreshTokenService.SaveChangesAsync();

            if (saveResult.Value is 0)
                throw new TokenPersistingException();

            return Result<AuthResult>.Success(BuildAuthResult(user, jwtToken, refreshToken));
        }

        private AuthResult BuildAuthResult(AppUser user, AccessTokenDto jwtToken, RefreshToken refreshToken)
        {
            var authResponse = new AuthResponse
            {
                AccessToken = jwtToken,
                User = _mapper.Map<UserDto>(user)
            };

            return new()
            {
                AuthResponse = authResponse,
                RefreshToken = refreshToken
            };
        }

        private bool IsTimeToRotateToken(RefreshToken refreshToken) =>
            (refreshToken.ExpiresAtUtc - DateTime.UtcNow) < TimeSpan.FromDays(_refreshTokenOptions.RotationThresholdDays);
    }
}
