using MongoDB.Driver;

namespace DessertsMakery.Common.Persistence.Mongo;

public interface IMongoEntityCollection<T> : IMongoCollection<T>
    where T : MongoEntity { }
