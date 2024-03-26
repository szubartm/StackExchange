namespace StackExchange.API.CustomExceptions;

public class StackExchangeException(long errorId, string errorName, string errorMessage) : Exception
{
    public readonly long ErrorId = errorId;
    public readonly string ErrorMessage = errorMessage;
    public readonly string ErrorName = errorName;
}