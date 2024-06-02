using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DessertsMakery.Essentials.SDK.Integration.Tests;

public abstract class SdkFixture : IAsyncLifetime
{
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();
    private IConfiguration? _configuration;
    private MongoDatabaseWrapper? _mongoWrapper;
    private IServiceProvider? _serviceProvider;

    protected abstract Assembly TestAssembly { get; }

    protected IConfiguration Configuration => _configuration!;

    protected abstract void ConfigureServices(IServiceCollection services);

    public T Resolve<T>()
        where T : notnull
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("Service provider was not initialized yet");
        }

        return _serviceProvider.GetRequiredService<T>();
    }

    Task IAsyncLifetime.InitializeAsync()
    {
        BuildConfiguration();
        BuildServiceProvider();
        return Task.CompletedTask;
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        if (_mongoWrapper is not null)
        {
            await _mongoWrapper.DisposeAsync();
        }

        if (_serviceProvider is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
        }

        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private void BuildConfiguration()
    {
        var userSecrets = new ConfigurationBuilder().AddUserSecrets(TestAssembly).Build();
        var url = userSecrets["MongoSettings:ConnectionString"]!;
        _mongoWrapper = new MongoDatabaseWrapper(url);
        _mongoWrapper.Initialize(TestAssembly);
        _configuration = new ConfigurationBuilder()
            .AddUserSecrets(TestAssembly)
            .AddInMemoryCollection(
                new KeyValuePair<string, string?>[] { new("MongoSettings:DatabaseName", _mongoWrapper.DatabaseName) }
            )
            .Build();
    }

    private void BuildServiceProvider()
    {
        ConfigureServices(_serviceCollection);
        _serviceCollection.AddScoped<IConfiguration>(_ => _configuration!);
        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }
}
