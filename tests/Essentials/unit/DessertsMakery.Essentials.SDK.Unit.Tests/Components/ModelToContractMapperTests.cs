using Ardalis.SmartEnum.AutoFixture;
using AutoFixture;
using DessertsMakery.Essentials.Persistence.Entities;
using DessertsMakery.Essentials.SDK.Components;
using FluentAssertions;

namespace DessertsMakery.Essentials.SDK.Unit.Tests.Components;

public class ModelToContractMapperTests
{
    private readonly ModelToContractMapper _mapper = new();

    [Fact]
    public void Map_WhenModelIsNull_ReturnsNull()
    {
        // Act
        var contract = _mapper.Map((Component?)null);

        // Assert
        contract.Should().BeNull();
    }

    [Fact]
    public void Map_WhenModelIsValid_ReturnsContract()
    {
        // Arrange
        var model = new Fixture().Customize(new SmartEnumCustomization()).Create<Component>();

        // Act
        var contract = _mapper.Map(model);

        // Assert
        contract.Should().BeEquivalentTo(ComponentMapping(model));
    }

    [Fact]
    public void Map_WhenModelsAreValid_ReturnsContracts()
    {
        // Arrange
        var models = new Fixture().Customize(new SmartEnumCustomization()).CreateMany<Component>().ToArray();

        // Act
        var contracts = _mapper.Map(models);

        // Assert
        contracts.Should().BeEquivalentTo(models.Select(ComponentMapping));
    }

    private static object ComponentMapping(Component model) =>
        new
        {
            model.Id,
            model.Name,
            Measuring = model.Measuring.Name,
            ComponentType = model.ComponentType.Name,
            model.ComponentParent?.ParentId,
            model.ComponentParent?.Proportion,
            model.Audit.CreatedAt,
            model.Audit.ModifiedAt
        };
}
