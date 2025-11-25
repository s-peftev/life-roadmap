using AutoMapper;
using LR.Application.AppResult;
using LR.Application.AppResult.Errors.User;
using LR.Application.AppResult.ResultData.Account;
using LR.Application.DTOs.Token;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Services;
using LR.Application.Interfaces.Utils;
using LR.Application.Requests.User;
using LR.Domain.Entities.Users;
using LR.Domain.Enums;
using LR.Infrastructure.Exceptions.RefreshToken;
using LR.Infrastructure.Exceptions.User;
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
        IOptions<FrontendOptions> frontendOptions,
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
        private readonly FrontendOptions _frontendOptions = frontendOptions.Value;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IUserProfileService _userProfileService = userProfileService;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
        private readonly IRequestInfoService _requestInfoService = requestInfoService;

        public async Task<Result<AuthResult>> RegisterAsync(
            UserRegisterDto userRegisterDto,
            CancellationToken ct = default)
        {
            var userCheck = await EnsureUserIsUniqueAsync(userRegisterDto);
            if (!userCheck.IsSuccess)
                return Result<AuthResult>.Failure(userCheck.Error);

            await _userProfileService.BeginTransactionAsync(ct);

            var userResult = await CreateUserAsync(userRegisterDto);
            var user = userResult.Value;

            await AssignDefaultRoleAsync(user);
            await CreateProfileAsync(user, userRegisterDto, ct);

            await _userProfileService.CommitTransactionAsync(ct);

            return await AuthenticateUserAsync(user, ct: ct);
        }

        public async Task<Result<AuthResult>> LoginAsync(
            UserLoginDto userLoginDto,
            CancellationToken ct = default)
        {
            var user = await _userManager.GetByUserNameWithRolesAsync(userLoginDto.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
                return Result<AuthResult>.Failure(UserErrors.LoginFailed);

            return await AuthenticateUserAsync(user, ct: ct);
        }

        public async Task<Result<AuthResult>> RefreshToken(
            string refreshTokenValue,
            CancellationToken ct = default)
        {
            var rtResult = await _refreshTokenService
                .GetByTokenValueAsync(refreshTokenValue, ct);
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
                var saveResult = await _refreshTokenService.SaveChangesAsync(ct);

                if (!saveResult.IsSuccess)
                    throw new TokenRevokingException();

                return await AuthenticateUserAsync(user, refreshToken.SessionId, ct);
            }

            var jwtToken = _tokenService
                .GenerateJwtToken(_mapper.Map<JwtGenerationDto>(user));

            return Result<AuthResult>.Success(BuildAuthResult(jwtToken, refreshToken));
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

        public async Task<Result<string>> GenerateEmailConfirmationCodeAsync(EmailCodeRequest emailCodeRequest)
        { 
            var user = await _userManager.FindByEmailAsync(emailCodeRequest.Email);
            if (user is null || user.UserName != emailCodeRequest.UserName)
                return Result<string>.Success("");
            // do not notify about wrong email/username

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return Result<string>.Success(code);
        }

        public async Task<Result> ConfirmEmailAsync(EmailConfirmationRequest emailConfirmationRequest)
        {
            var user = await _userManager.FindByEmailAsync(emailConfirmationRequest.Email);
            if (user is null)
                return Result.Failure(UserErrors.EmailConfirmationFailed);

            var result = await _userManager.ConfirmEmailAsync(user, emailConfirmationRequest.Code);

            if (!result.Succeeded)
                return Result.Failure(UserErrors.EmailConfirmationFailed);

            return Result.Success();
        }

        //todo after implementing Email service replace Task<Result<string>> with Task<Result>
        public async Task<Result<string>> GeneratePasswordResetTokenAsync(ForgotPasswordRequest forgotPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordRequest.Email);
            if (user is null || user.UserName != forgotPasswordRequest.UserName)
                return Result<string>.Success("");
            // do not notify about wrong email/username

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = $"{_frontendOptions.Url}/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            return Result<string>.Success(callbackUrl);
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var user = await _userManager.FindByIdAsync(resetPasswordRequest.UserId);
            if (user is null)
                return Result.Failure(UserErrors.PasswordResetFailed);

            var result = await _userManager
                .ResetPasswordAsync(user, resetPasswordRequest.Token, resetPasswordRequest.Password);
            if (!result.Succeeded)
                return Result.Failure(UserErrors.PasswordResetFailed);

            if (!string.IsNullOrEmpty(user.Email) && !user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> ChangeUsernameAsync(
            ChangeUsernameRequest changeUsernameRequest,
            string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            { 
                return Result.Failure(UserErrors.NotFound);
            }

            if (await _userManager.FindByNameAsync(changeUsernameRequest.UserName) is not null)
            {
                return Result.Failure(UserErrors.UsernameIsTaken);
            }

            user.UserName = changeUsernameRequest.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new ChangeUsernameException();

            return Result.Success();
        }

        public async Task<Result> ChangePasswordAsync(
            ChangePasswordRequest changePasswordRequest,
            string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound);
            }

            var result = await _userManager.ChangePasswordAsync(
                user, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);

            if (!result.Succeeded)
                return Result.Failure(UserErrors.WrongCurrentPassword);

            return Result.Success();
        }

        private async Task<Result> EnsureUserIsUniqueAsync(UserRegisterDto userRegisterDto)
        {
            if (await _userManager.FindByNameAsync(userRegisterDto.UserName) is not null)
                return Result.Failure(UserErrors.UsernameIsTaken);

            if (!string.IsNullOrEmpty(userRegisterDto.Email) &&
                await _userManager.FindByEmailAsync(userRegisterDto.Email) is not null)
                return Result.Failure(UserErrors.EmailIsTaken);

            return Result.Success();
        }

        private async Task<Result<AppUser>> CreateUserAsync(UserRegisterDto userRegisterDto)
        {
            var user = _mapper.Map<AppUser>(userRegisterDto);
            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

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
            AppUser user,
            UserRegisterDto userRegisterDto,
            CancellationToken ct = default)
        {
            var profile = _mapper.Map<UserProfile>(userRegisterDto);
            profile.UserId = user.Id;
            _userProfileService.Add(profile);

            var saveResult = await _userProfileService.SaveChangesAsync(ct);

            if (saveResult.Value is 0)
                throw new UserRegisterException();

            return Result<UserProfile>.Success(profile);
        }

        private async Task<Result<AuthResult>> AuthenticateUserAsync(
            AppUser user,
            Guid? sessionId = default,
            CancellationToken ct = default)
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
            var saveResult = await _refreshTokenService.SaveChangesAsync(ct);

            if (saveResult.Value is 0)
                throw new TokenPersistingException();

            return Result<AuthResult>.Success(BuildAuthResult(jwtToken, refreshToken));
        }

        private AuthResult BuildAuthResult(AccessTokenDto jwtToken, RefreshToken refreshToken)
        {
            return new()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken
            };
        }

        private bool IsTimeToRotateToken(RefreshToken refreshToken) =>
            (refreshToken.ExpiresAtUtc - DateTime.UtcNow) < TimeSpan.FromDays(_refreshTokenOptions.RotationThresholdDays);
    }
}
