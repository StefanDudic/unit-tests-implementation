using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing;

public class MachineDataProcessorTests : IDisposable
{
    private readonly FakeCoffeeCountStore _coffeeCountStore;
    private readonly MachineDataProcessor _machineDataProcessor;

    public MachineDataProcessorTests()
    {
        _coffeeCountStore = new();
        _machineDataProcessor = new(_coffeeCountStore);
    }

    [Fact]
    public void ShouldSaveCountPerCoffeeType()
    {
        // Arrange
        var items = new[]
        {
           new MachineDataItem("Cappuccino", new DateTime(2022,10,27,8,0,0)),
           new MachineDataItem("Cappuccino", new DateTime(2022,10,27,9,0,0)),
           new MachineDataItem("Espresso", new DateTime(2022,10,27,10,0,0))
        };

        // Act
        _machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);

        var item = _coffeeCountStore.SavedItems[0];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(2, item.Count);

        item = _coffeeCountStore.SavedItems[1];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }

    [Fact]
    public void ShouldIgnoreItemsThatAreNotNewer()
    {
        // Arrange
        var items = new[]
        {
           new MachineDataItem("Cappuccino", new DateTime(2022,10,27,8,0,0)),
           new MachineDataItem("Cappuccino", new DateTime(2022,10,27,7,0,0)), // ignored
           new MachineDataItem("Cappuccino", new DateTime(2022,10,27,7,10,0)), // ignored
           new MachineDataItem("Cappuccino", new DateTime(2022,10,27,9,10,0)), 
           new MachineDataItem("Espresso", new DateTime(2022,10,27,10,0,0)),
           new MachineDataItem("Espresso", new DateTime(2022,10,27,10,0,0)), // ignored
        };

        // Act
        _machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);

        var item = _coffeeCountStore.SavedItems[0];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(2, item.Count);

        item = _coffeeCountStore.SavedItems[1];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }

    [Fact]
    public void ShouldClearPreviouseCoffeeCount()
    {
        // Arrange
        var items = new[]
        {
           new MachineDataItem("Cappuccino", new DateTime(2022,10,27,8,0,0)),
        };

        // Act
        _machineDataProcessor.ProcessItems(items);
        _machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);
        foreach (var item in _coffeeCountStore.SavedItems)
        {
            Assert.Equal("Cappuccino", item.CoffeeType);
            Assert.Equal(1, item.Count);
        }
    }

    public void Dispose()
    {
        // This runs after every test
        // Example of cleanup here would be to remove a file we created during testing
    }
}

// We are creating a fake interface implementation
public class FakeCoffeeCountStore : ICoffeeCountStore
{
    public List<CoffeeCountItem> SavedItems { get; } = new();
    public void Save(CoffeeCountItem item)
    {
        SavedItems.Add(item);
    }
}
