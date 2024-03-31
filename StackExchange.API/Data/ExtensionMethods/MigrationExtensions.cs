using Microsoft.EntityFrameworkCore;
using StackExchange.API.Data.Contexts;

namespace StackExchange.API.Data.ExtensionMethods;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<TagsDbContext>();

        dbContext.Database.Migrate();
    }
}