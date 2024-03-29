using StackExchange.API.Entities;

namespace StackExchange.API.Interfaces;

public interface ITagClient
{
    IEnumerable<StackExchangeResponse<Tags>> GetTags(Filter filter, int numberOfTags, int pageSize);
}