namespace LR.Application.Interfaces.Utils
{
    public interface ICancellationTokenProvider
    {
        CancellationToken GetCancellationToken();
    }
}
