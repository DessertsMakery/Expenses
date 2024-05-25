namespace DessertsMakery.Common.Persistence.Mongo;

internal interface ICollectionNamingStrategy
{
    string GetName(Type type);
}
