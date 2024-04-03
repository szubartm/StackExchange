namespace StackExchange.API.ExternalApi.Models;

public class StackExchangeResponse<T>
{
    public bool Success { get; set; }
    public ResponseData<T> ResponseData { get; set; }
}