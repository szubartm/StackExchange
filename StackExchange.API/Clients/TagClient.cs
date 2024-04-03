using Newtonsoft.Json;
using StackExchange.API.ExternalApi.Models;
using StackExchange.API.Helpers;

namespace StackExchange.API.Clients;

public class TagClient(IHttpClientFactory factory, ILogger<TagClient> logger) : ITagClient
{
    private const int MaxPageSize = 100;
    private static string _apiUrl;

    public IEnumerable<ResponseData<Tags>> GetTags(StackExchangeQueryObject query)
    {
        var client = factory.CreateClient("TagClient");
        var responseList = new List<ResponseData<Tags>>();
        var currentPageSize = query.PageSize;

        if (!Validators.IsPageSizeValid(query.PageSize))
        {
            currentPageSize = MaxPageSize;
            logger.LogWarning(
                "Provided page size ({query.PageSize}) is not valid. Setting page size to the biggest possible ({MaxPageSize}).",
                query.PageSize, MaxPageSize);
        }

        while (query.NumberOfExpectedTags > 0)
        {
            if (query.NumberOfExpectedTags < currentPageSize) currentPageSize = query.NumberOfExpectedTags;

            _apiUrl =
                $"{client.BaseAddress}&{query.NumberOfExpectedTags}&page={query.PageNumber}&sort={query.SortBy}&order={query.Order}&pagesize={currentPageSize}&site=stackoverflow";

            var httpContent = client.GetAsync(_apiUrl).Result.Content;
            var response = JsonConvert.DeserializeObject<ResponseData<Tags>>(httpContent.ReadAsStringAsync().Result);

            if (response is null) break;

            responseList.Add(response);

            if (response.ErrorId is not null) break;
            if (!response.HasMore) break;

            query.PageNumber++;
            query.NumberOfExpectedTags -= currentPageSize;
        }

        return responseList;
    }
}