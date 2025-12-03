namespace LR.Application.Interfaces.Utils
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
