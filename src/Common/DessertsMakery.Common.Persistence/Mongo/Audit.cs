using DessertsMakery.Common.Utility.Extensions;

namespace DessertsMakery.Common.Persistence.Mongo;

public sealed class Audit
{
    public Audit()
    {
        var now = DateTime.UtcNow.Truncate(10_000);
        CreatedAt = now;
        ModifiedAt = now;
    }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; set; }
}
