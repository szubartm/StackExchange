using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StackExchange.API.Data.Contexts;
using StackExchange.API.Data.Entities;
using StackExchange.API.Data.Models;
using StackExchange.API.ExternalApi.Models;
using StackExchange.API.Helpers;
using StackExchange.API.Services;

namespace StackExchange.API.Repositories;

public class TagRepository(TagsDbContext context, ITagService service, ILogger<TagRepository> logger)
    : ITagRepository
{
    public async Task<IResponseDataModel<IEnumerable<TagDto>>> GetTags(DbQueryObject dbQuery)
    {
        var tags = context.Tags.AsQueryable();

        if (!string.IsNullOrWhiteSpace(dbQuery.SortBy))
        {
            if (dbQuery.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                tags = dbQuery.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? tags.OrderByDescending(tag => tag.Name)
                    : tags.OrderBy(tag => tag.Name);

            if (dbQuery.SortBy.Equals("share", StringComparison.OrdinalIgnoreCase))
                tags = dbQuery.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? tags.OrderByDescending(tag => tag.Share)
                    : tags.OrderBy(tag => tag.Share);
        }

        if (dbQuery.PageNumber <= 0)
        {
            logger.LogWarning("Invalid page number. Was {PageNumber}, setting to: 1", dbQuery.PageNumber);
            dbQuery.PageNumber = 1;
        }

        if (dbQuery.PageSize < 0)
        {
            logger.LogWarning("Invalid page size. Was {pagesize}, setting to: 25", dbQuery.PageSize);
            dbQuery.PageSize = 25;
        }

        var data = await tags.Skip((dbQuery.PageNumber - 1) * dbQuery.PageSize).Take(dbQuery.PageSize).ToListAsync();
        return data.Count > 0
            ? new ResponseDataModel<IEnumerable<TagDto>>
            {
                Success = true,
                Data = data
            }
            : new ResponseDataModel<IEnumerable<TagDto>>
            {
                Success = false,
                Message = "Tags not found"
            };
    }


    public async Task<IResponseDataModel<TagDto>> UpdateTag(TagDto tag)
    {
        var dataToUpdate = await GetTag(x => x.Id.Equals(tag.Id));
        ;
        if (!dataToUpdate.Success)
            return new ResponseDataModel<TagDto> { Success = false, Message = dataToUpdate.Message };

        context.Entry(dataToUpdate.Data).State = EntityState.Detached;
        dataToUpdate.Data = tag;
        context.Update(dataToUpdate.Data);

        if (await context.SaveChangesAsync() != 1)
            return new ResponseDataModel<TagDto>
            {
                Success = false
            };
        var test = context.Tags;


        service.SetPercentageOfAllGivenTags(context.Tags.ToList());
        await context.SaveChangesAsync();

        return new ResponseDataModel<TagDto>
        {
            Success = true,
            Data = dataToUpdate.Data
        };
    }

    public async Task<IResponseModel> DeleteTagById(int id)
    {
        var data = await GetTag(x => x.Id.Equals(id));
        if (!data.Success) return new ResponseModel { Success = false, Message = data.Message };
        context.Tags.Remove(data.Data);
        return await context.SaveChangesAsync() == 1
            ? new ResponseModel { Success = true }
            : new ResponseModel
            {
                Success = false
            };
    }

    public async Task<IResponseModel> DeleteTagByName(string name)
    {
        var data = await GetTag(x => x.Name.Equals(name));
        if (!data.Success) return new ResponseModel { Success = false, Message = data.Message };
        context.Tags.Remove(data.Data);
        return await context.SaveChangesAsync() == 1
            ? new ResponseModel { Success = true }
            : new ResponseModel
            {
                Success = false
            };
    }

    public async Task<IResponseDataModel<IEnumerable<TagDto>>> AddTags(List<Tags[]> tagsList)
    {
        if (tagsList.Count == 0)
            return new ResponseDataModel<IEnumerable<TagDto>>
            {
                Success = false,
                Data = new List<TagDto>()
            };

        var tagDto = new TagDto();
        var data = new List<TagDto>();
        foreach (var tags in tagsList)
        foreach (var tag in tags)
        {
            tagDto = tag;
            data.Add(tagDto);
        }

        await context.Truncate();
        await context.Tags.AddRangeAsync(data);
        if (await context.SaveChangesAsync() == 0)
            return new ResponseDataModel<IEnumerable<TagDto>>
            {
                Success = false,
                Data = new List<TagDto>()
            };

        service.SetPercentageOfAllGivenTags(context.Tags.ToList());
        await context.SaveChangesAsync();
        logger.LogInformation("Fetched tags: {count}", data.Count);
        return new ResponseDataModel<IEnumerable<TagDto>>
        {
            Success = true,
            Data = data
        };
    }

    public async Task<IResponseDataModel<TagDto>> GetTag(Expression<Func<TagDto, bool>> filter)
    {
        var data = await context.Tags.SingleOrDefaultAsync(filter);
        return data != null
            ? new ResponseDataModel<TagDto>
            {
                Success = true,
                Data = data
            }
            : new ResponseDataModel<TagDto>
            {
                Success = false,
                Message = "Tag not found"
            };
    }

    public async Task<IResponseDataModel<IEnumerable<TagDto>>> CreateTagsAsync(List<TagDto> tagsList)
    {
        await context.Tags.AddRangeAsync(tagsList);
        if (await context.SaveChangesAsync() == 0)
            return new ResponseDataModel<IEnumerable<TagDto>>
            {
                Success = false,
                Data = new List<TagDto>()
            };

        service.SetPercentageOfAllGivenTags(context.Tags.ToList());
        await context.SaveChangesAsync();
        logger.LogInformation("Added tags: {count}", tagsList.Count);
        return new ResponseDataModel<IEnumerable<TagDto>>
        {
            Success = true,
            Data = tagsList
        };
    }

    public async Task<IResponseDataModel<IEnumerable<TagDto>>> ListAllTagsAsync()
    {
        return new ResponseDataModel<IEnumerable<TagDto>>
        {
            Success = true,
            Data = await context.Tags.ToListAsync()
        };
    }
}