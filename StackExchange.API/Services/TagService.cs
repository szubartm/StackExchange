using Microsoft.EntityFrameworkCore;
using StackExchange.API.Data.Contexts;
using StackExchange.API.Data.Entities;
using StackExchange.API.Entities;
using StackExchange.API.Models.Api;

namespace StackExchange.API.Services;

public class TagService(IPercentageCalculator calculator, ILogger<TagService> logger, TagsDbContext dbContext)
    : ITagService
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

    public async Task SaveAsync(List<Tags[]> tagsList)
    {
        var tagDto = new TagDto();
        var tagDtosList = new List<TagDto>();
        foreach (var tags in tagsList)
        foreach (var tag in tags)
        {
            tagDto = tag;
            tagDtosList.Add(tagDto);
        }

        var missingTags = tagDtosList.Where(x => !dbContext.Tags.Any(z => z.Name == x.Name)).ToList();
        
        if (missingTags.Count == 0) return;
        
        await dbContext.Tags.AddRangeAsync(missingTags);
        await dbContext.SaveChangesAsync();
    }

    public List<TagDto> GetTags()
    {
        return dbContext.Tags.ToList();
    }
}