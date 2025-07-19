using LR.Infrastructure.DependencyInjection.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace LR.Infrastructure.DependencyInjection
{
    public static class ResolveDI
    {
        public static void AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            PersistanceResolver.AddPersistance(services, connectionString);
        }
    }
}
