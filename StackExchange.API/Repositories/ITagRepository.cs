using System.Linq.Expressions;
using StackExchange.API.Data.Entities;
using StackExchange.API.Data.Models;
using StackExchange.API.ExternalApi.Models;
using StackExchange.API.Helpers;

namespace StackExchange.API.Repositories;

public interface ITagRepository
{
    Task<IResponseDataModel<IEnumerable<TagDto>>> GetTags(DbQueryObject dbQuery);
    Task<IResponseDataModel<TagDto>> GetTag(Expression<Func<TagDto, bool>> filter);
    Task<IResponseDataModel<IEnumerable<TagDto>>> AddTags(List<Tags[]> tagsList);
    Task<IResponseDataModel<TagDto>> UpdateTag(TagDto tag);
    Task<IResponseModel> DeleteTagById(int id);
    Task<IResponseModel> DeleteTagByName(string name);

    //for populating test database
    Task<IResponseDataModel<IEnumerable<TagDto>>> CreateTagsAsync(List<TagDto> tagsList);
    Task<IResponseDataModel<IEnumerable<TagDto>>> ListAllTagsAsync();
}