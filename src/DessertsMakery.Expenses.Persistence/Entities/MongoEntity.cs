using MongoDB.Bson.Serialization.Attributes;

namespace DessertsMakery.Expenses.Persistence.Entities;

public class MongoEntity
{
    protected MongoEntity() { }

    [BsonId]
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Audit Audit { get; set; } = new();
}
