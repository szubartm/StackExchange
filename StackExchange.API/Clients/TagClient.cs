using Newtonsoft.Json;
using StackExchange.API.Entities;
using StackExchange.API.Helpers;
using StackExchange.API.Interfaces;

namespace StackExchange.API.Clients;

public partial class StackExchangeClient : ITagClient
{
    private static string _apiUrl;
    private static int _page = 1;
    private static readonly int _pageSize = 100;

    public ITagClient TagClient => this;
    //https://api.stackexchange.com/2.3/tags?order=desc&site=stackoverflow

    List<StackExchangeResponse<Tags>> ITagClient.GetThousandTags(Filter filter)
    {
        HttpContent? content = null;
        StackExchangeResponse<Tags>? response;
        var list = new List<StackExchangeResponse<Tags>>();
        var numberOfIterations = 1000 / _pageSize;
        for (var i = 0; i < numberOfIterations; i++)
        {
            _apiUrl = $"{_baseApiUrl}/tags?key={_apiKey}&page={_page}&site={filter.Site}&pagesize={_pageSize}";

            if (filter.Order is not null) _apiUrl += $"&order={filter.Order.ToString().ToLower()}";

            content = _httpClient.GetAsync(_apiUrl).Result.Content;

            response = JsonConvert.DeserializeObject<ResponseData<Tags>>(content.ReadAsStringAsync().Result)
                .ValidateResponse();

            if (response.ResponseData.HasMore) _page++;

            list.Add(response);
        }

        //var response = _httpClient.GetAsync(_apiUrl).Result.Content.ReadFromJsonAsync<StackExchangeResponse<Tags>>().ValidateResponse();
        var numberOfTags = list.Count * list.FirstOrDefault().ResponseData.Items.Length;
        _logger.LogInformation("Number fo tags: {0}",args: numberOfTags);
        return list;
    }
}