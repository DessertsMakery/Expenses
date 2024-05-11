using DessertsMakery.Expenses.Persistence.Entities;
using Humanizer;

namespace DessertsMakery.Expenses.Persistence.Collections;

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
