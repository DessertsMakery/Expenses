namespace DessertsMakery.Expenses.Persistence.Entities;

public sealed class Audit
{
    public Audit()
    {
        var now = DateTime.UtcNow;
        CreatedAt = now;
        ModifiedAt = now;
    }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; set; }
}
