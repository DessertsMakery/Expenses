using System.Reflection;
using DessertsMakery.Common.Persistence;
using DessertsMakery.Common.Utility.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DessertsMakery.Essentials.SDK;

public static class Dependencies
{
    private static readonly Assembly ThisAssembly = typeof(Dependencies).Assembly;

    public static IServiceCollection AddEssentialsSdk(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddServices(ThisAssembly);
        services.AddMappers(ThisAssembly);
        return services;
    }
}
