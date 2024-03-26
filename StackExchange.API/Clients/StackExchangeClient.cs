using System.Net;

namespace StackExchange.API.Clients;

public partial class StackExchangeClient
{
    private static HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseApiUrl;
    private readonly ILogger _logger;

    public StackExchangeClient(string apiKey, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) throw new Exception($"ApiKey null or empty : {nameof(apiKey)}");
        _apiKey = apiKey;
        _logger = logger;
        _baseApiUrl = "https://api.stackexchange.com/2.3";
        var httpClientHandler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip
        };
        _httpClient = new HttpClient(httpClientHandler);
    }
}