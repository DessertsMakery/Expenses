using Ardalis.SmartEnum;

namespace DessertsMakery.Expenses.Persistence.Entities;

public sealed class Measuring : SmartEnum<Measuring>
{
    public static readonly Measuring Quantity = new(nameof(Quantity), 1);
    public static readonly Measuring Mass = new(nameof(Mass), 2);

    private Measuring(string name, int value)
        : base(name, value) { }
}
