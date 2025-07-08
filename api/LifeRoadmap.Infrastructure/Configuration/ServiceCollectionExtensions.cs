using LifeRoadmap.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LifeRoadmap.Infrastructure.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString) 
        {
            services.AddDbContext<AppDbContext>(option 
                => option.UseSqlServer(connectionString));

            return services;
        }
    }
}
