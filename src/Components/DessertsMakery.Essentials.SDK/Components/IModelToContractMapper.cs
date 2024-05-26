using DessertsMakery.Essentials.Persistence.Entities;
using DessertsMakery.Essentials.SDK.Components.Contracts;

namespace DessertsMakery.Essentials.SDK.Components;

internal interface IModelToContractMapper
{
    ComponentDto? Map(Component? component);
    IEnumerable<ComponentDto> Map(IEnumerable<Component> components);
}
