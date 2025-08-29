using LR.Application.DTOs.Token;
using LR.Application.Interfaces.Utils;
using LR.Domain.Entities.Users;
using LR.Infrastructure.Constants.ExceptionMessages;
using LR.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LR.Infrastructure.Utils
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly SigningCredentials _signingCredentials;

        public TokenService(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor)
        {
            _jwtOptions = jwtOptions.Value;

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            _signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }

        public AccessTokenDto GenerateJwtToken(JwtGenerationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                throw new ArgumentException(TokensExceptionMessages.UserIdRequiredForJwt);

            if (string.IsNullOrWhiteSpace(dto.UserName))
                throw new ArgumentException(TokensExceptionMessages.UserNameRequiredForJwt);

            if (dto.Roles == null || dto.Roles.Count == 0)
                throw new ArgumentException(TokensExceptionMessages.RolesRequiredForJwt);

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, dto.UserId),
                new(ClaimTypes.Name, dto.UserName),
            };

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, dto.Email));
            }

            claims.AddRange(dto.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: _signingCredentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AccessTokenDto(jwtToken, expires);
        }

        public RefreshToken GenerateRefreshToken(RefreshTokenGenerationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                throw new ArgumentException(TokensExceptionMessages.UserIdRequiredForRefreshToken);

            if (dto.ExpirationDays <= 0)
                throw new ArgumentException(TokensExceptionMessages.ExpirationDaysPositiveRequired);

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
    }
}
