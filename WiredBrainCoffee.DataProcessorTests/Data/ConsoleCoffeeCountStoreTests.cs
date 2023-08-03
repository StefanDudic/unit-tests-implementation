using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Data;

public class ConsoleCoffeeCountStoreTests
{
    [Fact]
    public void ShouldWriteOutputToConsole()
    {
        // Arrange
        var item = new CoffeeCountItem("Cappuccino", 5);
        var stringWriter = new StringWriter();
        var consoleCoffeeCountStore = new ConsoleCoffeeCountStore(stringWriter);

        // Act
        consoleCoffeeCountStore.Save(item);

        // Assert
        var resault = stringWriter.ToString();
        Assert.Equal($"{item.CoffeeType}:{item.Count}{Environment.NewLine}", resault);
    }
}
