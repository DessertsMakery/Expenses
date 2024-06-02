using DessertsMakery.Essentials.SDK.Components;
using DessertsMakery.Essentials.SDK.Components.Contracts;
using FluentAssertions;
using FluentAssertions.Execution;

namespace DessertsMakery.Essentials.SDK.Integration.Tests.Components;

[Collection("SDK Collection")]
public sealed class CreateComponentTests(EssentialsSdkFixture sdkFixture)
{
    [Fact]
    public async Task CreateAsync_Always_ShouldCreateRecordInMongo()
    {
        // Arrange
        var service = sdkFixture.Resolve<IComponentService>();
        var createComponentDto = new CreateComponentDto("Milk", "Mass", "Consumable");

        // Act
        var actual = await service.CreateAsync(createComponentDto);

        // Assert
        using (new AssertionScope())
        {
            actual
                .Should()
                .BeEquivalentTo(
                    new
                    {
                        createComponentDto.Name,
                        createComponentDto.Measuring,
                        createComponentDto.ComponentType,
                        createComponentDto.ParentId,
                        createComponentDto.Proportion
                    }
                );
        }
    }

    [Fact]
    public async Task GetByNameAsync_WhenFewEntries_ShouldReturnThem()
    {
        // Arrange
        var service = sdkFixture.Resolve<IComponentService>();
        await service.CreateAsync(new CreateComponentDto("Milk", "Mass", "Consumable"));
        await service.CreateAsync(new CreateComponentDto("Jam", "Mass", "Consumable"));
        await service.CreateAsync(new CreateComponentDto("Ham", "Mass", "Consumable"));

        // Act
        var actual = await service.TryGetBestMatchByNameAsync("Sam");

        // Assert
        using (new AssertionScope())
        {
            actual.Should().NotBeEmpty().And.HaveCount(2);
            actual.Select(x => x.Name).Should().Contain(["Jam", "Ham"]);
        }
    }
}
