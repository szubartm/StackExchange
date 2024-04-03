namespace StackExchange.API.Services;

public class CountShareCalculator : ICalculator
{
    public decimal CalculatePercentage(long singleObjectCount, long collectionCount)
    {
        if (singleObjectCount < 0) throw new ArgumentException("First param must be positive!");
        if (collectionCount <= 0) throw new ArgumentException("Second param must be bigger than 0!");

        var percentage = Convert.ToDecimal(singleObjectCount) / Convert.ToDecimal(collectionCount) * 100;

        return Math.Round(percentage, 5);
    }
}