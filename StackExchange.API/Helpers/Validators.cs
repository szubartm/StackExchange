namespace StackExchange.API.Helpers;

public class Validators
{
    public static bool IsPageSizeValid(int pageSize)
    {
        return pageSize is <= 100 and > 0;
    }
}