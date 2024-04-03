using Microsoft.AspNetCore.Mvc;
using StackExchange.API.Data.Entities;
using StackExchange.API.Helpers;
using StackExchange.API.Repositories;

namespace StackExchange.API;

public static class TagEndpoints
{
    public static RouteGroupBuilder RegisterTagEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("", GetTag);
        group.MapGet("/all", GetAllTags);
        group.MapGet("id={id}", GetTagById);
        group.MapPost("", CreateTagsAsync);
        group.MapPut("", UpdateTag);
        group.MapDelete("id={id:int}", DeleteTagById);
        group.MapDelete("name={name}", DeleteTagByName);

        return group;
    }

    public static async Task<IResult> GetTag([AsParameters] DbQueryObject query, ITagRepository tagRepository)
    {
        var result = await tagRepository.GetTags(query);
        return result.Success ? TypedResults.Ok(result) : TypedResults.NotFound(result);
    }

    public static async Task<IResult> GetTagById(int id, ITagRepository tagRepository)
    {
        var result = await tagRepository.GetTag(x => x.Id == id);
        return result.Success ? TypedResults.Ok(result) : TypedResults.NotFound(result);
    }

    public static async Task<IResult> UpdateTag([FromBody] TagDto tag, ITagRepository tagRepository)
    {
        var result = await tagRepository.UpdateTag(tag);
        return result.Success ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
    }

    public static async Task<IResult> DeleteTagById(ITagRepository tagRepository, int id)
    {
        var result = await tagRepository.DeleteTagById(id);
        return result.Success ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
    }

    public static async Task<IResult> DeleteTagByName(ITagRepository tagRepository, string name)
    {
        var result = await tagRepository.DeleteTagByName(name);
        return result.Success ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
    }

    public static async Task<IResult> CreateTagsAsync(ITagRepository tagRepository, List<TagDto> tagDtos)
    {
        var result = await tagRepository.CreateTagsAsync(tagDtos);
        return result.Success ? TypedResults.Created("/api/tags", result) : TypedResults.BadRequest(result);
    }

    public static async Task<IResult> GetAllTags(ITagRepository tagRepository)
    {
        var result = await tagRepository.ListAllTagsAsync();
        return result.Success ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
    }
}