using DessertsMakery.Common.Utility.Algorithms;
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

    public async Task<Component[]> TryGetBestMatchByNameAsync(string name, CancellationToken token = default)
    {
        var exactComponent = await componentCollection.Find(x => x.Name == name).FirstOrDefaultAsync(token);
        if (exactComponent is not null)
        {
            return [exactComponent];
        }

        var allComponents = await componentCollection.Find(Builders<Component>.Filter.Empty).ToListAsync(token);

        const int maximumDistance = 2;
        return allComponents
            .Select(component => new
            {
                Distance = DamerauLevenshteinAlgorithm.Distance(component.Name, name, maximumDistance),
                Component = component
            })
            .Where(x => x.Distance <= maximumDistance)
            .OrderBy(x => x.Distance)
            .ThenBy(x => x.Component.Name)
            .Select(x => x.Component)
            .ToArray();
    }

    public async Task<ComponentDto> CreateAsync(CreateComponentDto createComponentDto, CancellationToken token)
    {
        var component = modelToContractMapper.Map(createComponentDto);
        await componentCollection.InsertOneAsync(component, options: null, token);
        return modelToContractMapper.Map(component)!;
    }

    public async Task<bool> DeleteAsync(Guid guid, CancellationToken token = default)
    {
        var result = await componentCollection.DeleteOneAsync(x => x.Id == guid, token);
        return result.DeletedCount > 0;
    }
}
