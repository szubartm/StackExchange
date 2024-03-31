using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.API.Clients;
using StackExchange.API.Data.Contexts;
using StackExchange.API.Entities;
using StackExchange.API.Enums;
using StackExchange.API.Interfaces;
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

app.UseHttpsRedirection();

app.MapGet("/Test", async (ILogger<Program> logger, HttpResponse response) =>
{
    logger.LogInformation("Testing logging in Program.cs");
    await response.WriteAsync("Testing");
}).WithName("Test").WithOpenApi();

app.MapGet("/tags",
    async ([FromServices] ITagService tagService, [FromServices] ITagClient tagClient, int count = 2, int pagesize = 100) =>
    {
        var result = tagClient.GetTags(new Filter(Order.Desc), count, pagesize);
        tagService.SetPercentageOfAllGivenTags(result);
        var test = result.Select(x => x.ResponseData).Select(y => y.Items).ToList();

        await tagService.SaveAsync(test);

        return result;
    });

app.MapGet("/result", ([FromServices] ITagService tagService) =>
{
    return tagService.GetTags();
});
app.Run();

void Configure(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddEndpointsApiExplorer();
    webApplicationBuilder.Services.AddSwaggerGen();
    webApplicationBuilder.Services.AddScoped<IPercentageCalculator, PercentageCalculator>();
    webApplicationBuilder.Services.AddScoped<ITagClient, TagClient>();
    webApplicationBuilder.Services.AddScoped<ITagService, TagService>();
    webApplicationBuilder.Services.AddHttpClient("TagClient", client =>
    {
        var apiKey = webApplicationBuilder.Configuration["ApiKey"];
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
    webApplicationBuilder.Services.AddDbContext<TagsDbContext>(options =>
    {
        options.UseNpgsql(webApplicationBuilder.Configuration["Database"]);
    });
    webApplicationBuilder.Logging.AddConsole();
    webApplicationBuilder.Configuration.AddUserSecrets<Program>();
}