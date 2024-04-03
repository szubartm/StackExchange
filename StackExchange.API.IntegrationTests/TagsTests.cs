using System.Net;
using System.Net.Http.Json;
using StackExchange.API.Data.Entities;
using StackExchange.API.IntegrationTests.Helpers;

namespace StackExchange.API.IntegrationTests;

public class TagTests : IAsyncLifetime
{
    private readonly TestWebApplicationFactory _factory;
    private HttpClient _client;

    public TagTests()
    {
        _factory = new TestWebApplicationFactory();
    }

    public async Task InitializeAsync()
    {
        await _factory.InitializeAsync();
        _client = _factory.CreateClient();
        await _client.GetAsync(
            "/populate-database?NumberOfExpectedTags=10&SortBy=name&Order=desc&PageNumber=1&PageSize=100");
    }

    public Task DisposeAsync()
    {
        _client.Dispose();
        return _factory.DisposeAsync();
    }

    [Fact]
    public async Task UpdateTagInDatabase()
    {
        // Arrange
        var obj = new TagDto
            { Id = 2, Name = "test2", HasSynonyms = true, Count = 100, IsRequired = false, IsModeratorOnly = true };
        // Act
        var response = await _client.PostAsJsonAsync("/api/tags", obj);
        var tag = await _client.GetFromJsonAsync<TagDto>("/api/tags/&id=2");

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        Assert.NotNull(tag);
        Assert.Equal("test2", tag.Name);
        Assert.Equal(2, tag.Id);
        Assert.Equal(100, tag.Count);
        Assert.False(tag.IsRequired);
        Assert.True(tag.IsModeratorOnly);
    }
}