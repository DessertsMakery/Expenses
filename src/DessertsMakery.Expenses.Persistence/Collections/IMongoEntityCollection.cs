using DessertsMakery.Expenses.Persistence.Entities;
using MongoDB.Driver;

namespace DessertsMakery.Expenses.Persistence.Collections;

public interface IMongoEntityCollection<T> : IMongoCollection<T>
    where T : MongoEntity { }
