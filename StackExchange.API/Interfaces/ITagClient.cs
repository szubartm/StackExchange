using StackExchange.API.Entities;
using StackExchange.API.Models.Api;

namespace StackExchange.API.Interfaces;

public interface ITagClient
{
    IEnumerable<StackExchangeResponse<Tags>> GetTags(Filter filter, int numberOfTags, int pageSize);
}