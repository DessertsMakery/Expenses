using Humanizer;

namespace DessertsMakery.Common.Persistence.Mongo;

internal sealed class CollectionNamingStrategy : ICollectionNamingStrategy
{
    private static readonly Type MongoEntityType = typeof(MongoEntity);

    public string GetName(Type type)
    {
        if (!MongoEntityType.IsAssignableFrom(type))
        {
            throw new InvalidOperationException(
                $"Type `{type}` should be inherited from `{MongoEntityType}` to be transformed to a collection name"
            );
        }

        return type.Name.Pluralize();
    }
}
