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
            component.Measuring.Name,
            component.ComponentType.Name,
            component.ComponentParent?.Proportion,
            component.ComponentParent?.ParentId,
            component.Audit.CreatedAt,
            component.Audit.ModifiedAt
        );
    }

    public IEnumerable<ComponentDto> Map(IEnumerable<Component> components) => components.Select(Map)!;

    public Component Map(CreateComponentDto createComponentDto)
    {
        var (name, measuring, componentType, parentId, proportion) = createComponentDto;
        ThrowIfProportionNotPositiveDecimal(proportion);
        ThrowIfProportionIsDefinedWithoutParent(parentId, proportion);

        var component = new Component
        {
            Name = name,
            Measuring = Measuring.FromName(measuring),
            ComponentType = ComponentType.FromName(componentType)
        };

        if (!parentId.HasValue)
        {
            return component;
        }

        const int defaultProportion = 1;
        component.ComponentParent = new ComponentParent
        {
            ParentId = parentId.Value,
            Proportion = proportion.GetValueOrDefault(defaultProportion)
        };
        return component;
    }

    private static void ThrowIfProportionIsDefinedWithoutParent(Guid? parentId, decimal? proportion)
    {
        if (!parentId.HasValue && proportion.HasValue)
        {
            throw new InvalidOperationException("Proportion can only be used when parent is defined");
        }
    }

    private static void ThrowIfProportionNotPositiveDecimal(decimal? proportion)
    {
        if (proportion is <= 0)
        {
            throw new InvalidOperationException("Proportion should be a positive number");
        }
    }
}
