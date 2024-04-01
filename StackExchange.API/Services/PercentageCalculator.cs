namespace StackExchange.API.Services;

public class PercentageCalculator : IPercentageCalculator
{
    public decimal CalculatePercentageShareInTagCollection(long singleTagCount, long collectionCount)
    {
        var percentage = Convert.ToDecimal(singleTagCount) / Convert.ToDecimal(collectionCount) * 100;

        return Math.Round(percentage, 5);
    }
}