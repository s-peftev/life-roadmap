using LR.Application.DTOs.Token;
using LR.Domain.Entities.Users;

namespace LR.Application.AppResult.ResultData.Account
{
    public class AuthResult
    {
        public required AccessTokenDto AccessToken { get; init; }
        public required RefreshToken RefreshToken { get; init; }
    }
}
