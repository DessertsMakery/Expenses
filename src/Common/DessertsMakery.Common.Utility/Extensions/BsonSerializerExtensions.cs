using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization;

namespace DessertsMakery.Common.Utility.Extensions;

public static class BsonSerializerExtensions
{
    public static IServiceCollection AddCustomBsonSerializing(this IServiceCollection services, Assembly[] assemblies)
    {
        services.RegisterAllNonAbstractNonGenericServices<IBsonSerializer>(assemblies);
        services.RegisterAllNonAbstractNonGenericServices<IBsonSerializationProvider>(assemblies);
        return services;
    }

    public static IApplicationBuilder UseCustomBsonSerializing(this IApplicationBuilder builder)
    {
        var serializers = builder.ApplicationServices.GetServices<IBsonSerializer>();
        foreach (var serializer in serializers)
        {
            BsonSerializer.RegisterSerializer(serializer.ValueType, serializer);
        }

        var providers = builder.ApplicationServices.GetServices<IBsonSerializationProvider>();
        foreach (var provider in providers)
        {
            BsonSerializer.RegisterSerializationProvider(provider);
        }

        return builder;
    }

    private static void RegisterAllNonAbstractNonGenericServices<TMarker>(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies
    )
    {
        var marker = typeof(TMarker);
        var serializerTypes = assemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(BsonSerializerIsNonAbstractNonGenericClass);
        foreach (var serializerType in serializerTypes)
        {
            services.TryAddSingleton(marker, serializerType);
        }

        bool BsonSerializerIsNonAbstractNonGenericClass(TypeInfo type) =>
            type is { IsClass: true, IsAbstract: false, IsGenericType: false } && marker.IsAssignableFrom(type);
    }
}
