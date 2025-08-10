using LR.Domain.Interfaces;
using LR.Domain.Interfaces.Repositories;
using LR.Persistance;
using LR.Persistance.Repositories;
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

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        }
    }
}
