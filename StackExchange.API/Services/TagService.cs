using StackExchange.API.Entities;

namespace StackExchange.API.Services;

public class TagService(IPercentageCalculator calculator, ILogger<TagService> logger) : ITagService
{
    public void SetPercentageOfAllGivenTags(IEnumerable<StackExchangeResponse<Tags>> response)
    {
        var items = response.Select(x => x.ResponseData).Select(y => y.Items);

        var tagsCount = items.Sum(tag => tag.Sum(x => x.Count));
        logger.LogInformation("Total number of tags => {0}", tagsCount);

        foreach (var tags in items)
        foreach (var tag in tags)
            tag.PercentageOfAllGivenTags = calculator.CalculatePercentageShareInTagCollection(tag.Count, tagsCount);
    }
}