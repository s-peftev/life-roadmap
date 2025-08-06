namespace LR.Application.Interfaces.Utils
{
    public interface ITokenService
    {
        (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken();
    }
}
