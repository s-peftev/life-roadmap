using LR.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LR.Infrastructure.DependencyInjection.Resolvers
{
    internal static class PersistanceResolver
    {
        internal static void AddPersistance(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
