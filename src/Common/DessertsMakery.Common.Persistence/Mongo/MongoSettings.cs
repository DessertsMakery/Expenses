namespace DessertsMakery.Common.Persistence.Mongo;

public sealed class MongoSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
