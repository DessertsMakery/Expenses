using System.Reflection;
using DessertsMakery.Expenses.Common.Extensions;
using DessertsMakery.Expenses.Persistence.Collections;
using DessertsMakery.Expenses.Persistence.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DessertsMakery.Expenses.Persistence.DependencyInjection;

public static class Dependencies
{
    private static readonly Assembly ThisAssembly = typeof(Dependencies).Assembly;

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguration<MongoSettings>(configuration);
        services.TryAddSingleton<IMongoClient>(MongoClientFactory);
        services.TryAddScoped(MongoDatabaseFactory);
        services.TryAddTransient<ICollectionNamingStrategy, CollectionNamingStrategy>();
        services.TryAddMongoCollections();
        return services;
    }

    private static MongoClient MongoClientFactory(IServiceProvider provider)
    {
        var internalSettings = provider.GetRequiredService<IOptions<MongoSettings>>().Value;
        var clientSettings = MongoClientSettings.FromConnectionString(internalSettings.ConnectionString);
        return new MongoClient(clientSettings);
    }

    private static IMongoDatabase MongoDatabaseFactory(IServiceProvider provider)
    {
        var internalSettings = provider.GetRequiredService<IOptions<MongoSettings>>().Value;
        var mongoClient = provider.GetRequiredService<IMongoClient>();
        return mongoClient.GetDatabase(internalSettings.DatabaseName);
    }

    private static void TryAddMongoCollections(this IServiceCollection services)
    {
        var marker = typeof(MongoEntity);
        var entityTypes = ThisAssembly.DefinedTypes.Where(MongoEntityIsPublicNonAbstractClass).ToArray();
        foreach (var entityType in entityTypes)
        {
            var mongoCollectionType = typeof(IMongoCollection<>).MakeGenericType(entityType);
            services.TryAddScoped(mongoCollectionType, provider => provider.MongoCollectionFactory(entityType));
        }

        bool MongoEntityIsPublicNonAbstractClass(TypeInfo type) =>
            type is { IsClass: true, IsAbstract: false, IsPublic: true }
            && marker.IsAssignableFrom(type)
            && type != marker;
    }

    private static object MongoCollectionFactory(this IServiceProvider provider, Type entityType)
    {
        var collectionNamingStrategy = provider.GetRequiredService<CollectionNamingStrategy>();
        var collectionName = collectionNamingStrategy.GetName(entityType);

        var mongoDatabase = provider.GetRequiredService<IMongoDatabase>();
        var openGenericMethod = typeof(IMongoDatabase).GetMethod(nameof(IMongoDatabase.GetCollection))!;
        var getCollectionMethod = openGenericMethod.MakeGenericMethod(entityType);

        var arguments = new object?[] { collectionName, null };
        var collection = getCollectionMethod.Invoke(mongoDatabase, arguments)!;
        return collection;
    }
}
