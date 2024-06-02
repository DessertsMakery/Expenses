using System.Collections.Concurrent;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace DessertsMakery.Common.Persistence.Mongo.Extensions;

public static class MongoCollectionExtensions
{
    private static readonly ConcurrentDictionary<string, byte> IndexCache = new();

    public static async Task CreateTextIndexAsync<T>(
        this IMongoCollection<T> collection,
        Expression<Func<T, object>> selector,
        CancellationToken token = default
    )
    {
        var indexKey = CreateIndexKey(collection, selector);
        if (IndexCache.ContainsKey(indexKey))
        {
            return;
        }

        var indexKeysDefinition = Builders<T>.IndexKeys.Text(selector);
        var indexModel = new CreateIndexModel<T>(
            indexKeysDefinition,
            new CreateIndexOptions { Unique = true, Name = indexKey }
        );
        await collection.Indexes.CreateOneAsync(indexModel, cancellationToken: token);
        IndexCache.TryAdd(indexKey, byte.MinValue);
    }

    private static string CreateIndexKey<T>(IMongoCollection<T> collection, Expression<Func<T, object>> selector)
    {
        var collectionName = collection.CollectionNamespace.CollectionName;
        var selectorBody = ((MemberExpression)selector.Body).Member.Name;
        var indexKey = $"{collectionName}-{selectorBody}";
        return indexKey;
    }
}
