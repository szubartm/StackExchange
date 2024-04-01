using StackExchange.API.Data.Entities;

namespace StackExchange.API.Services;

public class TagService(IPercentageCalculator calculator, ILogger<TagService> logger)
    : ITagService
{
    public void SetPercentageOfAllGivenTags(List<TagDto> tags)
    {
        var tagsCount = tags.Sum(tag => tag.Count);

        foreach (var tag in tags)
            tag.Share = calculator.CalculatePercentageShareInTagCollection(tag.Count, tagsCount);
    }
}