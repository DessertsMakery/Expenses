using DessertsMakery.Components.SDK.Components.Contracts;
using DessertsMakery.Expenses.Persistence.Entities;
using MongoDB.Driver;

namespace DessertsMakery.Components.SDK.Components;

internal sealed class ComponentService : IComponentService
{
    private readonly IMongoCollection<Component> _componentCollection;

    public ComponentService(IMongoCollection<Component> componentCollection)
    {
        _componentCollection = componentCollection;
    }

    public Task<Component?> GetByIdAsync(Guid guid, CancellationToken token) =>
        _componentCollection.Find(x => x.Id == guid).FirstOrDefaultAsync(token)!;

    public async Task<Component> CreateAsync(CreateComponentDto createComponentDto, CancellationToken token)
    {
        var (name, proportion) = createComponentDto;
        var component = new Component { Name = name, Proportion = proportion };
        await _componentCollection.InsertOneAsync(component, options: null, token);
        return component;
    }

    public async Task<bool> DeleteAsync(Guid guid, CancellationToken token = default)
    {
        var result = await _componentCollection.DeleteOneAsync(x => x.Id == guid, token);
        return result.DeletedCount > 0;
    }
}
