namespace StackExchange.API.Services;

public class PercentageCalculator : IPercentageCalculator
{
    public decimal CalculatePercentageShareInTagCollection(long singleTagCount, long collectionCount)
    {
        return Convert.ToDecimal(singleTagCount) / Convert.ToDecimal(collectionCount) * 100;
    }
}