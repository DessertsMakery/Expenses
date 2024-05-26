using DessertsMakery.Common.Persistence;
using DessertsMakery.Common.Utility.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DessertsMakery.Components.SDK;

public static class Dependencies
{
    public static IServiceCollection AddComponentsSdk(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddServices(typeof(Dependencies).Assembly);
        return services;
    }
}
