using MongoDB.Bson.Serialization.Attributes;

namespace DessertsMakery.Common.Persistence.Mongo;

public class MongoEntity
{
    protected MongoEntity() { }

    [BsonId]
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Audit Audit { get; set; } = new();
}
