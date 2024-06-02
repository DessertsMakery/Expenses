using Ardalis.SmartEnum;

namespace DessertsMakery.Essentials.Persistence.Entities;

public sealed class ComponentType : SmartEnum<ComponentType>
{
    public static readonly ComponentType Consumable = new(nameof(Consumable), 1);
    public static readonly ComponentType Decoration = new(nameof(Decoration), 2);
    public static readonly ComponentType Inventory = new(nameof(Inventory), 3);
    public static readonly ComponentType Packaging = new(nameof(Packaging), 4);

    private ComponentType(string name, int value)
        : base(name, value) { }
}