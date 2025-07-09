using LifeRoadmap.Application.Configuration.Mapping;
using LifeRoadmap.Application.Interfaces.Persistence;
using LifeRoadmap.Database;
using LifeRoadmap.Infrastructure.Configuration.Mapping;
using LifeRoadmap.Infrastructure.Persistence;
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

            services.AddScoped<IUnitOfWork, UnitOfwork>();

            return services;
        }

        public static IServiceCollection AddAddMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(EntityToDomainProfile).Assembly);
            services.AddAutoMapper(typeof(DomainToDtoProfile).Assembly);

            return services;
        }
    }
}
