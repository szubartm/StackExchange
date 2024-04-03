using StackExchange.API.Services;

namespace StackExchange.Api.UnitTests;

public class ShareCalculatorTests
{
    [Fact]
    public async Task Calculator_ReturnCorrectValue_WhenInputsAreValid()
    {
        var calculator = new CountShareCalculator();
        decimal expected = 10;

        var result = calculator.CalculatePercentage(10, 100);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task Calculator_ThrowInvalidDataException_WhenFirstParameterIsInvalid()
    {
        var calculator = new CountShareCalculator();

        var result = Assert.Throws<ArgumentException>(() => calculator.CalculatePercentage(-1, 100));

        Assert.NotNull(result);
        Assert.IsType<ArgumentException>(result);
        Assert.Equal("First param must be positive!", result.Message);
    }

    [Fact]
    public async Task Calculator_ThrowsArgumentException_WhenSecondParameterIsInvalid()
    {
        var calculator = new CountShareCalculator();

        var result = Assert.Throws<ArgumentException>(() => calculator.CalculatePercentage(4, -130));

        Assert.NotNull(result);
        Assert.IsType<ArgumentException>(result);
        Assert.Equal("Second param must be bigger than 0!", result.Message);
    }
}