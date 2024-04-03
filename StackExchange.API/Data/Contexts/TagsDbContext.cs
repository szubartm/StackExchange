using Microsoft.EntityFrameworkCore;
using StackExchange.API.Data.Entities;

namespace StackExchange.API.Data.Contexts;

public class TagsDbContext : DbContext
{
    public TagsDbContext()
    {
    }

    public TagsDbContext(DbContextOptions<TagsDbContext> options) : base(options)
    {
    }

    public virtual DbSet<TagDto> Tags { get; set; }

    public async Task Truncate()
    {
        await Database.ExecuteSqlAsync($"TRUNCATE ONLY Tags");
    }
}