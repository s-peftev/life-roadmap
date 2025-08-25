using LR.Application.DTOs.Token;
using LR.Application.Interfaces.Utils;
using LR.Domain.Entities.Users;
using LR.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace LR.Infrastructure.Utils
{
    public class TokenService(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor) 
        : ITokenService
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public AccessTokenDto GenerateJwtToken(TokenUserDto tokenUserDto)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, tokenUserDto.Id.ToString()),
                new Claim(ClaimTypes.Name, tokenUserDto.UserName),
            };

            if (!string.IsNullOrWhiteSpace(tokenUserDto.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, tokenUserDto.Email));
            }

            claims.AddRange(tokenUserDto.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AccessTokenDto(jwtToken, expires);
        }

        public RefreshToken GenerateRefreshToken(RefreshTokenGenerationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                throw new ArgumentException("UserId is required.");

            if (dto.ExpirationDays <= 0)
                throw new ArgumentException("ExpirationDays must be positive.");

            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            
            return new() 
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresAtUtc = DateTime.UtcNow.AddDays(dto.ExpirationDays),
                UserAgent = dto.UserAgent,
                IpAddress = dto.IpAddress,
                SessionId = dto.SessionId ?? Guid.NewGuid(),
                UserId = dto.UserId
            };
        }

        public void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, 
            DateTime expiration)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(cookieName,
                token, new CookieOptions
                { 
                    HttpOnly = true,
                    Expires = expiration,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
        }
    }
}
