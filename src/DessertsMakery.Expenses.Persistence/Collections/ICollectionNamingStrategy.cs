namespace DessertsMakery.Expenses.Persistence.Collections;

internal interface ICollectionNamingStrategy
{
    string GetName(Type type);
}
