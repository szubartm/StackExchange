namespace StackExchange.API.Services;

public interface IPercentageCalculator
{
    decimal CalculatePercentageShareInTagCollection(long singleTagCount, long collectionCount);
}