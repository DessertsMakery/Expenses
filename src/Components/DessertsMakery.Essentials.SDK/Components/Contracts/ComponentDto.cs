using DessertsMakery.Essentials.Persistence.Entities;

namespace DessertsMakery.Essentials.SDK.Components.Contracts;

public sealed record ComponentDto(
    Guid Id,
    string Name,
    Measuring Measuring,
    decimal? Proportion,
    Guid? ParentId,
    DateTime CreatedAt,
    DateTime ModifiedAt
);
