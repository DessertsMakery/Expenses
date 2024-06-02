using System.Reflection;
using MongoDB.Driver;

namespace DessertsMakery.Essentials.SDK.Integration.Tests;

internal sealed class MongoDatabaseWrapper(string url) : IAsyncDisposable
{
    private const string DessertsMakery = nameof(DessertsMakery);
    private const string IntegrationTests = "Integration.Tests";
    private readonly MongoClient _mongoClient = new(url);
    private IMongoDatabase? _mongoDatabase;

    private IMongoDatabase MongoDatabase
    {
        get
        {
            if (_mongoDatabase is null)
            {
                throw new InvalidOperationException("Mongo database was not initialized yet");
            }

            return _mongoDatabase;
        }
    }

    public string DatabaseName => MongoDatabase.DatabaseNamespace.DatabaseName;

    public void Initialize(Assembly assembly)
    {
        var fullName = assembly.GetName().Name!;
        if (!fullName.StartsWith(DessertsMakery))
        {
            throw new InvalidOperationException(
                $"Cannot create database for assembly not starting with prefix `{DessertsMakery}`"
            );
        }

        if (!fullName.EndsWith(IntegrationTests))
        {
            throw new InvalidOperationException(
                $"Cannot create database for assembly not ending with suffix `{IntegrationTests}`"
            );
        }

        var shortName = fullName[DessertsMakery.Length..^IntegrationTests.Length].Trim('.').Replace('.', '_');
        var databaseName = $"IT_{shortName}_{DateTime.UtcNow:s}";
        _mongoDatabase = _mongoClient.GetDatabase(databaseName);
    }

    public async ValueTask DisposeAsync()
    {
        if (_mongoDatabase != null)
        {
            await _mongoClient.DropDatabaseAsync(MongoDatabase.DatabaseNamespace.DatabaseName);
        }
    }
}
