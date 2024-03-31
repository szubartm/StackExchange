namespace StackExchange.API.Entities;

public class StackExchangeResponse<T>
{
    public bool Success { get; set; }
    public Exception Exception { get; set; }
    public ResponseData<T> ResponseData { get; set; }
}