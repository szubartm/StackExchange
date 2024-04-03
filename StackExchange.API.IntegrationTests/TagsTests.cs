using System.Net;
using System.Net.Http.Json;
using StackExchange.API.Data.Entities;
using StackExchange.API.Data.Models;
using StackExchange.API.IntegrationTests.Helpers;

namespace StackExchange.API.IntegrationTests;

public class TagTests : IAsyncLifetime
{
    private readonly TestWebApplicationFactory _factory;

    private readonly List<TagDto> _tagDtos =
    [
        new TagDto
            { Id = 1, Name = "test1", HasSynonyms = true, Count = 100, IsRequired = false, IsModeratorOnly = true },
        new TagDto
            { Id = 2, Name = "test2", HasSynonyms = false, Count = 230, IsRequired = false, IsModeratorOnly = true },
        new TagDto
            { Id = 3, Name = "test3", HasSynonyms = true, Count = 413, IsRequired = true, IsModeratorOnly = true },
        new TagDto
            { Id = 4, Name = "test4", HasSynonyms = false, Count = 11, IsRequired = false, IsModeratorOnly = false },
        new TagDto
            { Id = 5, Name = "test5", HasSynonyms = true, Count = 66, IsRequired = true, IsModeratorOnly = false }
    ];

    private HttpClient _client;

    public TagTests()
    {
        _factory = new TestWebApplicationFactory();
    }

    public async Task InitializeAsync()
    {
        await _factory.InitializeAsync();
        _client = _factory.CreateClient();
        await _client.PostAsJsonAsync("/api/tags", _tagDtos);
    }

    public Task DisposeAsync()
    {
        _client.Dispose();
        return _factory.DisposeAsync();
    }

    [Fact]
    public async Task CanGetTagsWithQueryParams()
    {
        //Act
        var response =
            await _client.GetFromJsonAsync<ResponseDataModel<List<TagDto>>>(
                "/api/tags?SortBy=null&Order=desc&PageNumber=2&PageSize=2");

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Equal(2, response.Data.Count);
        Assert.Equal(_tagDtos[2].Name, response.Data[0].Name);
        Assert.Equal(_tagDtos[3].IsModeratorOnly, response.Data[1].IsModeratorOnly);
    }

    [Fact]
    public async Task CanGetAllTags()
    {
        var response = await _client.GetFromJsonAsync<ResponseDataModel<List<TagDto>>>("/api/tags/all");

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Equal(_tagDtos.Count, response.Data.Count);
    }

    [Fact]
    public async Task CanGetTagById()
    {
        var response = await _client.GetFromJsonAsync<ResponseDataModel<TagDto>>("/api/tags/id=1");

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Equal(_tagDtos[0].Id, response.Data.Id);
        Assert.Equal(_tagDtos[0].Name, response.Data.Name);
        Assert.Equal(_tagDtos[0].IsModeratorOnly, response.Data.IsModeratorOnly);
        Assert.Equal(_tagDtos[0].IsRequired, response.Data.IsRequired);
        Assert.Equal(_tagDtos[0].Count, response.Data.Count);
    }


    [Fact]
    public async Task CanUpdateTag()
    {
        // Arrange
        var obj = new TagDto
        {
            Id = 2, Name = "changedName", HasSynonyms = true, Count = 341, IsRequired = false, IsModeratorOnly = true
        };
        // Act

        var response = await _client.PutAsJsonAsync("/api/tags", obj);
        var tag = await _client.GetFromJsonAsync<ResponseDataModel<TagDto>>("/api/tags/id=2");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(tag);
        Assert.Equal("changedName", tag.Data.Name);
        Assert.Equal(2, tag.Data.Id);
        Assert.Equal(341, tag.Data.Count);
        Assert.False(tag.Data.IsRequired);
        Assert.True(tag.Data.IsModeratorOnly);
    }

    [Fact]
    public async Task CanDeleteTagById()
    {
        // Act
        var response = await _client.DeleteAsync("/api/tags/id=3");
        var tag = await _client.GetAsync("/api/tags/id=3");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, tag.StatusCode);
    }

    [Fact]
    public async Task CanDeleteTagByName()
    {
        // Act
        var response = await _client.DeleteAsync("/api/tags/name=test1");
        var tag = await _client.GetAsync("/api/tags/name=test1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, tag.StatusCode);
    }
}