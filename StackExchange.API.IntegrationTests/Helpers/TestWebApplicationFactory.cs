using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.API.Data.Contexts;
using StackExchange.API.Repositories;
using Testcontainers.PostgreSql;

namespace StackExchange.API.IntegrationTests.Helpers;

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithHostname("stackexchange.database")
        .WithDatabase("tags")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
    }


    public Task DisposeAsync()
    {
        return _postgres.DisposeAsync().AsTask();
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Remove(services.SingleOrDefault(service =>
                typeof(DbContextOptions<TagsDbContext>) == service.ServiceType));
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddDbContext<TagsDbContext>(options => options.UseNpgsql(_postgres.GetConnectionString()));
        });
    }
}