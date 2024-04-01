using System.Net;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.API.Clients;
using StackExchange.API.Data.Contexts;
using StackExchange.API.Data.Entities;
using StackExchange.API.Data.ExtensionMethods;
using StackExchange.API.Entities;
using StackExchange.API.Enums;
using StackExchange.API.Interfaces;
using StackExchange.API.Repositories;
using StackExchange.API.Services;

var builder = WebApplication.CreateBuilder(args);

Configure(builder);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.ApplyMigrations();
}

app.UseHttpLogging();
app.UseHttpsRedirection();
app.ApplyMigrations();

app.MapHealthChecks("_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapGet("/populate-database",
        async ([FromServices] ITagService tagService, [FromServices] ITagClient tagClient,
            [FromServices] ITagRepository tagRepository, int count = 1000,
            int pagesize = 100) =>
        {
            try
            {
                var result = tagClient.GetTags(new Filter(Order.Desc), count, pagesize);
                var tags = result.Select(x => x.ResponseData).Select(y => y.Items).ToList();
                
                await tagRepository.AddTags(tags);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }).WithTags("Fetch from official StackExchangeAPI")
    .WithDescription("Use to populate/refresh local db. Every call truncates table 'tags' in database.").WithOpenApi();

app.MapGet("/api/tags", async ([FromServices] ITagRepository tagRepository) =>
{
    var tags = await tagRepository.GetTags();
    return tags.Any()
        ? Results.Ok(tags.OrderBy(x=>x.Id))
        : Results.NotFound();
}).WithTags("From database");


app.MapGet("/api/tags/{id:int}", async ([FromServices] ITagRepository tagRepository, int id) =>
await tagRepository.GetTag(id)
    is TagDto tag
    ? Results.Ok(tag)
    : Results.NotFound()).WithTags("From database");

app.MapPut("/api/tags/", async ([FromBody] TagDto tag, [FromServices] ITagRepository tagRepository) =>
{
    await tagRepository.UpdateTag(tag);

    return Results.NoContent();
}).WithTags("From database");

app.MapDelete("/api/tags/{id:int}", async ([FromServices] ITagRepository tagRepository, int id) =>
{
    await tagRepository.DeleteTagById(id);

    return Results.NoContent();
}).WithTags("From database");

app.MapDelete("/api/tags/{name}", async ([FromServices] ITagRepository tagRepository, string name) =>
{
    await tagRepository.DeleteTagByName(name);

    return Results.NoContent();
}).WithTags("From database");
app.Run();

void Configure(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<IPercentageCalculator, PercentageCalculator>();
    builder.Services.AddScoped<ITagClient, TagClient>();
    builder.Services.AddScoped<ITagService, TagService>();
    builder.Services.AddScoped<ITagRepository, TagRepository>();
    builder.Services.AddHttpClient("TagClient", client =>
    {
        var apiKey = builder.Configuration["ApiKey"];
        if (apiKey is null)
            throw new ArgumentNullException();

        client.BaseAddress = new Uri($"https://api.stackexchange.com/2.3/tags?key={apiKey}");
    }).ConfigurePrimaryHttpMessageHandler(messageHandler =>
    {
        var httpClientHandler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
        };
        return httpClientHandler;
    });
    builder.Services.AddHttpLogging(log => log.CombineLogs = true);
    builder.Services.AddDbContext<TagsDbContext>(options => { options.UseNpgsql(builder.Configuration["Database"]); });
    builder.Logging.AddConsole();
    builder.Configuration.AddUserSecrets<Program>();
    builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration["Database"])
        .AddDbContextCheck<TagsDbContext>();
}