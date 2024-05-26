using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DessertsMakery.Common.Utility.Extensions;

public static class ServiceCollectionExtensions
{
    public const string Service = nameof(Service);
    private static readonly Assembly ThisAssembly = typeof(ServiceCollectionExtensions).Assembly;

    public static IServiceCollection AddServices(
        this IServiceCollection services,
        Assembly assembly,
        string suffix = Service
    ) => services.AddServices(new[] { assembly });

    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        string suffix = Service
    )
    {
        var implementations = assemblies.SelectMany(x => x.DefinedTypes).Where(ServiceNonAbstractClass).ToArray();
        foreach (var implementation in implementations)
        {
            foreach (var @interface in implementation.ImplementedInterfaces)
            {
                services.TryAddScoped(@interface, implementation);
            }
        }

        return services;

        bool ServiceNonAbstractClass(TypeInfo type) =>
            type is { IsClass: true, IsAbstract: false } && type.Name.EndsWith(suffix);
    }
}
