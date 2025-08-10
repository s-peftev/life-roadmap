using LR.Application.Validators;
using LR.Infrastructure.DependencyInjection.Resolvers;
using LR.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LR.Infrastructure.DependencyInjection
{
    public static class ResolveDI
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;

            PersistanceResolver.AddPersistance(services, connectionString);
            IdentityResolver.AddIdentityServices(services, configuration);
            UtilsResolver.AddUtils(services);
            ServiceResolver.AddServices(services);
            AutoMapperRegistration.RegisterProfiles(services);
            ValidatorsRegistration.RegisterValidators(services);
            ConfigurationResolver.AddConfigurationServices(services, configuration);
        }
    }
}
