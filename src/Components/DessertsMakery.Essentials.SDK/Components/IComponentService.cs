using DessertsMakery.Essentials.SDK.Components.Contracts;

namespace DessertsMakery.Essentials.SDK.Components;

public interface IComponentService
{
    Task<ComponentDto?> GetByIdAsync(Guid guid, CancellationToken token = default);
    Task<IEnumerable<ComponentDto>> GetByNameAsync(string name, CancellationToken token = default);
    Task<ComponentDto> CreateAsync(CreateComponentDto createComponentDto, CancellationToken token = default);
    Task<bool> DeleteAsync(Guid guid, CancellationToken token = default);
}
