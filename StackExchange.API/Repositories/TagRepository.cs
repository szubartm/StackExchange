using Microsoft.EntityFrameworkCore;
using StackExchange.API.Data.Contexts;
using StackExchange.API.Data.Entities;
using StackExchange.API.Helpers;
using StackExchange.API.Models.Api;
using StackExchange.API.Services;

namespace StackExchange.API.Repositories;

public class TagRepository(TagsDbContext context, ITagService tagService, ILogger<TagRepository> logger)
    : ITagRepository
{
    public async Task<IEnumerable<TagDto>> GetTags(DbQueryObject dbQuery)
    {
        var tags = context.Tags.AsQueryable();

        if (!string.IsNullOrWhiteSpace(dbQuery.SortBy))
        {
            if (dbQuery.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                tags = dbQuery.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? tags.OrderByDescending(tag => tag.Name)
                    : tags.OrderBy(tag => tag.Name);

            if (dbQuery.SortBy.Equals("share", StringComparison.OrdinalIgnoreCase))
                tags = dbQuery.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? tags.OrderByDescending(tag => tag.Share)
                    : tags.OrderBy(tag => tag.Share);
        }

        if (dbQuery.PageNumber <= 0)
        {
            logger.LogWarning("Invalid page number. Was {PageNumber}, setting to: 1", dbQuery.PageNumber);
            dbQuery.PageNumber = 1;
        }

        if (dbQuery.PageSize < 0)
        {
            logger.LogWarning("Invalid page size. Was {pagesize}, setting to: 25", dbQuery.PageSize);
            dbQuery.PageSize = 25;
        }

        return await tags.Skip((dbQuery.PageNumber - 1) * dbQuery.PageSize).Take(dbQuery.PageSize).ToListAsync();
    }

    public async Task<TagDto> GetTag(int id)
    {
        return await context.Tags.FindAsync(id);
    }

    public async Task AddTags(List<Tags[]> tagsList)
    {
        var tagDto = new TagDto();
        var tagDtosList = new List<TagDto>();
        foreach (var tags in tagsList)
        foreach (var tag in tags)
        {
            tagDto = tag;
            tagDtosList.Add(tagDto);
        }

        //var missingTags = tagDtosList.Where(x => !dbContext.Tags.Any(z => z.Name == x.Name)).ToList();

        //if (missingTags.Count == 0) return;
        //tagService.SetPercentageOfAllGivenTags(tagDtosList);
        await context.Truncate();
        await context.Tags.AddRangeAsync(tagDtosList);
        await context.SaveChangesAsync();
        await context.UpdatePercentageColumn();
        logger.LogInformation("Fetched tags: {count}", tagDtosList.Count);
    }


    public async Task UpdateTag(TagDto tag)
    {
        var tagToUpdate = await context.Tags.FindAsync(tag.Id);

        if (tagToUpdate is not null)
        {
            context.Entry(tagToUpdate).State = EntityState.Detached;
            tagToUpdate = tag;
            context.Update(tagToUpdate);
            await context.UpdatePercentageColumn();
        }
    }

    public async Task DeleteTagById(int id)
    {
        await context.Tags.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task DeleteTagByName(string name)
    {
        await context.Tags.Where(x => x.Name.Equals(name)).ExecuteDeleteAsync();
    }
}