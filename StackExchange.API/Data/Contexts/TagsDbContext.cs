using Microsoft.EntityFrameworkCore;
using StackExchange.API.Data.Entities;

namespace StackExchange.API.Data.Contexts;

public class TagsDbContext(DbContextOptions<TagsDbContext> options) : DbContext(options)
{
    public DbSet<TagDto> Tags { get; set; }
}