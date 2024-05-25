using DessertsMakery.Components.SDK.Components.Contracts;
using DessertsMakery.Expenses.Persistence.Entities;

namespace DessertsMakery.Components.SDK.Components;

public interface IComponentService
{
    Task<Component?> GetByIdAsync(Guid guid, CancellationToken token = default);
    Task<Component> CreateAsync(CreateComponentDto createComponentDto, CancellationToken token = default);
    Task<bool> DeleteAsync(Guid guid, CancellationToken token = default);
}
