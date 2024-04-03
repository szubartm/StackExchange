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
        group.MapGet("&id={id}", GetTagById);
        group.MapPost("", UpdateTag);
        group.MapDelete("&id={id:int}", DeleteTagById);
        group.MapDelete("&name={name}", DeleteTagByName);

        return group;
    }

    public static async Task<IResult> GetTag([AsParameters] DbQueryObject query, ITagRepository tagRepository)
    {
        var tags = await tagRepository.GetTags(query);
        return tags.Any()
            ? TypedResults.Ok(tags)
            : TypedResults.NotFound();
    }
    
    public static async Task<IResult> GetTagById(int id, ITagRepository tagRepository)
    {
        return await tagRepository.GetTag(id)
            is TagDto tag
            ? TypedResults.Ok(tag)
            : TypedResults.NotFound();
    }

    public static async Task<IResult> UpdateTag([FromBody] TagDto tag, ITagRepository tagRepository)
    {
        try
        {
            await tagRepository.UpdateTag(tag);
            return TypedResults.Created("/api/tags",tag);
        }
        catch(Exception e)
        {
            return TypedResults.NotFound(e.Data);
        }
    }

    public static async Task<IResult> DeleteTagById(ITagRepository tagRepository, int id)
    {
        try
        {
            await tagRepository.DeleteTagById(id);
            return TypedResults.NoContent();
        }
        catch (Exception e)
        {
            return TypedResults.NotFound(e);
        }
    }

    public static async Task<IResult> DeleteTagByName(ITagRepository tagRepository, string name)
    {
        try
        {
            await tagRepository.DeleteTagByName(name);
            return TypedResults.NoContent();
        }
        catch (Exception e)
        {
            return TypedResults.NotFound(e);
        }
    }
}
