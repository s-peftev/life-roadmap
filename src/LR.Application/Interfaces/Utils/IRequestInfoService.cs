namespace LR.Application.Interfaces.Utils
{
    public interface IRequestInfoService
    {
        string? GetIpAddress();
        string? GetUserAgent();
    }
}
