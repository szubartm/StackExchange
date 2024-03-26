using StackExchange.API.Clients;
using StackExchange.API.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped(hc => new StackExchangeClient("hBuHT5Zxe1PjfBdTGpt4iQ"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/Test", async (ILogger<Program> logger, HttpResponse response) =>
{
    logger.LogInformation("Testing logging in Program.cs");
    await response.WriteAsync("Testing");
}).WithName("Test").WithOpenApi();
using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information)); // Set to Information level

var client = new StackExchangeClient("hBuHT5Zxe1PjfBdTGpt4iQ((", factory.CreateLogger("StackExchangeClient"));
app.MapGet("/thousand-tag", () => { return client.TagClient.GetThousandTags(new Filter()); });
app.Run();