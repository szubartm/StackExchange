namespace StackExchange.API.Models.Api;

public class StackExchangeResponse<T>
{
    public bool Success { get; set; }
    public ResponseData<T> ResponseData { get; set; }
}