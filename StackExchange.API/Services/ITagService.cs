using StackExchange.API.Data.Entities;
using StackExchange.API.Entities;
using StackExchange.API.Models.Api;

namespace StackExchange.API.Services;

public interface ITagService
{
    void SetPercentageOfAllGivenTags(IEnumerable<StackExchangeResponse<Tags>> response);
    Task SaveAsync(List<Tags[]> tags);
    List<TagDto> GetTags();
}