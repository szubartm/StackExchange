using Newtonsoft.Json;
using StackExchange.API.Entities;
using StackExchange.API.Helpers;
using StackExchange.API.Interfaces;
using StackExchange.API.Models.Api;

namespace StackExchange.API.Clients;

public class TagClient(IHttpClientFactory factory, ILogger<TagClient> logger) : ITagClient
{
    private const int MaxPageSize = 100;
    private static string _apiUrl;

    public IEnumerable<StackExchangeResponse<Tags>> GetTags(Filter filter, int numberExpectedOfTags, int pageSize)
    {
        var client = factory.CreateClient("TagClient");
        var responseList = new List<StackExchangeResponse<Tags>>();
        int currentPageSize;
        if (Validators.IsPageSizeValid(pageSize))
        {
            currentPageSize = pageSize;
        }
        else
        {
            currentPageSize = MaxPageSize;
            logger.LogWarning(
                "Provided page size ({pageSize}) is not valid. Setting page size to the biggest possible ({MaxPageSize}).",
                pageSize, MaxPageSize);
        }

        var numberOfCurrentTags = 0;
        var page = 1;

        while (numberExpectedOfTags > 0)
        {
            if (numberExpectedOfTags < currentPageSize) currentPageSize = numberExpectedOfTags;

            _apiUrl =
                $"{client.BaseAddress}&{numberExpectedOfTags}&page={page}&site={filter.Site}&pagesize={currentPageSize}";

            if (filter.Order is not null) _apiUrl += $"&order={filter.Order.ToString().ToLower()}";

            var httpContent = client.GetAsync(_apiUrl).Result.Content;
            var response = JsonConvert.DeserializeObject<ResponseData<Tags>>(httpContent.ReadAsStringAsync().Result)
                .ValidateResponse();
            responseList.Add(response);

            numberExpectedOfTags -= currentPageSize;

            if (response.ResponseData.HasMore)
                page++;
            else
                break;
        }

        numberOfCurrentTags = responseList.SelectMany(x => x.ResponseData.Items.Select(y => y.Name)).Count();
        logger.LogInformation("Number of different tags fetched: {numberOfCurrentTags}", numberOfCurrentTags);
        logger.LogInformation("Number of batches: {responseList}", responseList.Count);

        return responseList;
    }
}