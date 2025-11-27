namespace LR.Application.Interfaces.Utils
{
    public interface IRefreshTokenCookieWriter
    {
        void Set(string refreshToken, DateTime expires);
        void Delete();
    }
}