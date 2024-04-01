using StackExchange.API.Data.Entities;
using StackExchange.API.Helpers;
using StackExchange.API.Models.Api;

namespace StackExchange.API.Repositories;

public interface ITagRepository
{
    Task<IEnumerable<TagDto>> GetTags(DbQueryObject dbQuery);
    Task<TagDto> GetTag(int id);
    Task AddTags(List<Tags[]> tagsList);
    Task UpdateTag(TagDto tag);
    Task DeleteTagById(int id);
    Task DeleteTagByName(string name);
}