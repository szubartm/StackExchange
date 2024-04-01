using Newtonsoft.Json;
using StackExchange.API.Entities;
using StackExchange.API.Helpers;
using StackExchange.API.Models.Api;

namespace StackExchange.API.Clients;

public class TagClient(IHttpClientFactory factory, ILogger<TagClient> logger) : ITagClient
{
    private const int MaxPageSize = 100;
    private static string _apiUrl;

    public IEnumerable<ResponseData<Tags>> GetTags(StackExchangeQueryObject query)
    {
        var client = factory.CreateClient("TagClient");
        var responseList = new List<ResponseData<Tags>>();
        int currentPageSize;
        if (Validators.IsPageSizeValid(query.PageSize))
        {
            currentPageSize = query.PageSize;
        }
        else
        {
            currentPageSize = MaxPageSize;
            logger.LogWarning(
                "Provided page size ({query.PageSize}) is not valid. Setting page size to the biggest possible ({MaxPageSize}).",
                query.PageSize, MaxPageSize);
        }

        var numberOfCurrentTags = 0;

        while (query.NumberOfExpectedTags > 0)
        {
            if (query.NumberOfExpectedTags < currentPageSize) currentPageSize = query.NumberOfExpectedTags;

            _apiUrl =
                $"{client.BaseAddress}&{query.NumberOfExpectedTags}&page={query.PageNumber}&sort={query.SortBy}&order={query.Order}&pagesize={currentPageSize}&site=stackoverflow";

            //if (filter.Order is not null) _apiUrl += $"&order={filter.Order.ToString().ToLower()}";

            var httpContent = client.GetAsync(_apiUrl).Result.Content;
            var response = JsonConvert.DeserializeObject<ResponseData<Tags>>(httpContent.ReadAsStringAsync().Result);


            responseList.Add(response);

            if (response.ErrorId is not null) break;

            query.NumberOfExpectedTags -= currentPageSize;


            if (response.HasMore)
                query.PageNumber++;
            else
                break;
        }

        // numberOfCurrentTags = responseList.SelectMany(x => x.ResponseData.Items.Select(y => y.Name)).Count();
        // logger.LogInformation("Number of different tags fetched: {numberOfCurrentTags}", numberOfCurrentTags);
        // logger.LogInformation("Number of batches: {responseList}", responseList.Count);

        return responseList;
    }
}