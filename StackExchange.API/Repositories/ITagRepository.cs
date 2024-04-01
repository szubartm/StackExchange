using StackExchange.API.Data.Entities;
using StackExchange.API.Models.Api;

namespace StackExchange.API.Repositories;

public interface ITagRepository
{
    Task<IEnumerable<TagDto>> GetTags();
    Task<TagDto> GetTag(int id);
    Task AddTags(List<Tags[]> tagsList);
    Task UpdateTag(TagDto tag);
    Task DeleteTagById(int id);
    Task DeleteTagByName(string name);
}