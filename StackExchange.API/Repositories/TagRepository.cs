using Microsoft.EntityFrameworkCore;
using StackExchange.API.Data.Contexts;
using StackExchange.API.Data.Entities;
using StackExchange.API.Models.Api;
using StackExchange.API.Services;

namespace StackExchange.API.Repositories;

public class TagRepository(TagsDbContext context, ITagService tagService) : ITagRepository
{
    private readonly TagsDbContext _context = context;

    public async Task<IEnumerable<TagDto>> GetTags()
    {
        return await _context.Tags.ToListAsync();
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
        var tagToDelete = context.Tags.FirstOrDefaultAsync(x => x.Id == id).Result;
        if (tagToDelete is not null)
        {
            context.Tags.Remove(tagToDelete);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteTagByName(string name)
    {
        var tagToDelete = context.Tags.FirstOrDefaultAsync(x => x.Name == name).Result;
        if (tagToDelete is not null)
        {
            context.Tags.Remove(tagToDelete);
            await context.SaveChangesAsync();
        }
    }
}