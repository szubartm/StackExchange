using StackExchange.API.Entities;

namespace StackExchange.API.Interfaces;

public interface ITagClient
{
    List<StackExchangeResponse<Tags>> GetThousandTags(Filter filter);
}