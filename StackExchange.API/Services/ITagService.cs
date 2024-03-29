using StackExchange.API.Entities;

namespace StackExchange.API.Services;

public interface ITagService
{
    void SetPercentageOfAllGivenTags(IEnumerable<StackExchangeResponse<Tags>> response);
}