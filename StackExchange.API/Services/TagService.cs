using StackExchange.API.Data.Entities;

namespace StackExchange.API.Services;

public class TagService(ICalculator calculator)
    : ITagService
{
    public List<TagDto> SetPercentageOfAllGivenTags(List<TagDto> tags)
    {
        if (tags.Count == 0) return tags;

        var tagsCount = tags.Sum(tag => tag.Count);

        foreach (var tag in tags)
            tag.Share = calculator.CalculatePercentage(tag.Count, tagsCount);

        return tags;
    }
}