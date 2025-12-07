using LR.Application.Interfaces.ExternalProviders;
using LR.Infrastructure.ExternalProviders;
using Microsoft.Extensions.DependencyInjection;

namespace LR.Infrastructure.DependencyInjection.Resolvers
{
    internal class ExternalProvidersResolver
    {
        internal static void AddExternalProviders(IServiceCollection services)
        {
            services.AddScoped<IPhotoService, CloudinaryPhotoService>();
        }
    }
}
