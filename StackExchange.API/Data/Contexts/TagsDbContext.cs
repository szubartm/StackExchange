using Microsoft.EntityFrameworkCore;
using StackExchange.API.Data.Entities;
using StackExchange.API.Services;

namespace StackExchange.API.Data.Contexts;

public class TagsDbContext(DbContextOptions<TagsDbContext> options, ITagService tagService) : DbContext(options)
{
    public DbSet<TagDto> Tags { get; set; }

    public async Task Truncate()
    {
        await Database.ExecuteSqlAsync($"TRUNCATE ONLY Tags");
    }

    public async Task UpdatePercentageColumn()
    {
        tagService.SetPercentageOfAllGivenTags(await Tags.ToListAsync());
        await SaveChangesAsync();
    }
    
}