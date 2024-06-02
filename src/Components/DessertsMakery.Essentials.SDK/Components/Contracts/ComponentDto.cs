namespace DessertsMakery.Essentials.SDK.Components.Contracts;

public sealed record ComponentDto(
    Guid Id,
    string Name,
    string Measuring,
    string ComponentType,
    decimal? Proportion,
    Guid? ParentId,
    DateTime CreatedAt,
    DateTime ModifiedAt
);
