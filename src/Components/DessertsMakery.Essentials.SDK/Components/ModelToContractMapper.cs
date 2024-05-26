using DessertsMakery.Essentials.Persistence.Entities;
using DessertsMakery.Essentials.SDK.Components.Contracts;

namespace DessertsMakery.Essentials.SDK.Components;

internal sealed class ModelToContractMapper : IModelToContractMapper
{
    public ComponentDto? Map(Component? component)
    {
        if (component is null)
        {
            return null;
        }

        return new ComponentDto(
            component.Id,
            component.Name,
            component.Measuring,
            component.Proportion,
            component.ParentId,
            component.Audit.CreatedAt,
            component.Audit.ModifiedAt
        );
    }

    public IEnumerable<ComponentDto> Map(IEnumerable<Component> components) => components.Select(Map)!;
}
