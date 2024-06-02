using BenchmarkDotNet.Attributes;
using Bogus;
using DessertsMakery.Essentials.Persistence.Entities;
using DessertsMakery.Essentials.SDK.Components;
using MongoDB.Driver;
using Moq;
using Soenneker.Utils.AutoBogus;

namespace DessertsMakery.Essentials.SDK.Benchmarks.Components;

[RPlotExporter]
[MemoryDiagnoser]
public class ComponentServiceBenchmarks
{
    private const int Seed = 42;
    private const int ComponentCount = 5_000;

    private static readonly Random Random = new(Seed);

    private static readonly Faker<Component> ComponentFaker = new AutoFaker<Component>()
        .Rules((faker, component) => component.Name = faker.Commerce.Product())
        .UseSeed(Seed);

    private static readonly string RandomComponentCorruptedName;
    private static readonly ComponentService ComponentService;

    static ComponentServiceBenchmarks()
    {
        var componentCollectionMock = new Mock<IMongoCollection<Component>>();
        ComponentService = new ComponentService(componentCollectionMock.Object, new ModelToContractMapper());

        var asyncCursorMockForDirectSearch = new Mock<IAsyncCursor<Component>>();
        componentCollectionMock
            .Setup(x =>
                x.FindAsync(
                    It.IsAny<FilterDefinition<Component>>(),
                    It.IsAny<FindOptions<Component, Component>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(asyncCursorMockForDirectSearch.Object);
        asyncCursorMockForDirectSearch.Setup(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var asyncCursorMockForFuzzySearch = new Mock<IAsyncCursor<Component>>();
        componentCollectionMock
            .Setup(x =>
                x.FindAsync(
                    FilterDefinition<Component>.Empty,
                    It.IsAny<FindOptions<Component, Component>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(asyncCursorMockForFuzzySearch.Object);

        var index = 0;
        asyncCursorMockForFuzzySearch
            .Setup(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => index++ % 2 == 0);
        var components = ComponentFaker.Generate(ComponentCount);
        asyncCursorMockForFuzzySearch.Setup(x => x.Current).Returns(() => components);

        var randomIndex = Random.Next(0, ComponentCount);
        RandomComponentCorruptedName = CorruptNameOf(components[randomIndex]);
    }

    [Benchmark]
    public Task<Component[]> TryGetBestMatchByNameAsync() =>
        ComponentService.TryGetBestMatchByNameAsync(RandomComponentCorruptedName);

    private static string CorruptNameOf(Component component)
    {
        var name = component.Name.ToCharArray();
        Random.Shuffle(name);
        return new string(name);
    }
}
