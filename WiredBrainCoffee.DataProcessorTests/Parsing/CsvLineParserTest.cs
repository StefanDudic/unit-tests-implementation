namespace WiredBrainCoffee.DataProcessor.Parsing;

public class CsvLineParserTest
{
    //attribute to make it a test method
    [Fact]
    public void ShouldParseValidLine()
    {
        // Arrange
        //initialization of a test property
        string[] csvLines = new[] { "Cappuccino;10/27/2022 8:06:04 AM" };

        // Act
        //method we are testing
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // Assert
        //Assert contains methods for thesting
        Assert.NotNull(machineDataItems);
        Assert.Single(machineDataItems);
        Assert.Equal("Cappuccino", machineDataItems[0].CoffeeType);
        Assert.Equal(new DateTime(2022, 10, 27, 8, 6, 4), machineDataItems[0].CreatedAt);
    }

    [Fact]
    public void ShouldSkipEmptyLines()
    {
        // Arrange
        string[] csvLines = new[] { "", " " };

        // Act
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // Assert
        Assert.NotNull(machineDataItems);
        // Checking if the value is empty
        Assert.Empty(machineDataItems);
    }

    [InlineData("Cappuccino", "Invalid csv line")]
    [InlineData("Cappuccino;InvalidDateTime", "Invalid datetime in csv line")]
    [Theory]
    public void ShouldThrowExceptionForInvalidLine(string csvLine, string expectedMessagePrefix)
    {
        // Arrange
        string[] csvLines = new[] { csvLine };

        // Act
        // Test for a generic exception
        var exception = Assert.Throws<Exception>(() => CsvLineParser.Parse(csvLines));

        // Assert
        Assert.Equal ($"{ expectedMessagePrefix}: {csvLine}", exception.Message);
    }
}
