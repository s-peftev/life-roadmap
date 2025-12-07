using LR.Application.Interfaces.Utils;

namespace LR.Infrastructure.Utils
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
