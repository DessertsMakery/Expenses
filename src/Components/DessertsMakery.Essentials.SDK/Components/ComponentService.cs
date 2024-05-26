using DessertsMakery.Essentials.Persistence.Entities;
using DessertsMakery.Essentials.SDK.Components.Contracts;
using MongoDB.Driver;

namespace DessertsMakery.Essentials.SDK.Components;

internal sealed class ComponentService(
    IMongoCollection<Component> componentCollection,
    IModelToContractMapper modelToContractMapper
) : IComponentService
{
    public async Task<ComponentDto?> GetByIdAsync(Guid guid, CancellationToken token)
    {
        var component = await componentCollection.Find(x => x.Id == guid).FirstOrDefaultAsync(token);
        return modelToContractMapper.Map(component);
    }

    public async Task<IEnumerable<ComponentDto>> GetByNameAsync(string name, CancellationToken token = default)
    {
        var searchDefinition = Builders<Component>.Search.Text(x => x.Name, name);
        var component = await componentCollection
            .Aggregate()
            .Search(searchDefinition, scoreDetails: true)
            .ToListAsync(token);
        return modelToContractMapper.Map(component);
    }

    public async Task<ComponentDto> CreateAsync(CreateComponentDto createComponentDto, CancellationToken token)
    {
        var (name, proportion) = createComponentDto;
        var component = new Component { Name = name, Proportion = proportion };
        await componentCollection.InsertOneAsync(component, options: null, token);
        return modelToContractMapper.Map(component)!;
    }

    public async Task<bool> DeleteAsync(Guid guid, CancellationToken token = default)
    {
        var result = await componentCollection.DeleteOneAsync(x => x.Id == guid, token);
        return result.DeletedCount > 0;
    }
}
