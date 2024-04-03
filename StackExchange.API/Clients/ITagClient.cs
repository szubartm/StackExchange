using StackExchange.API.ExternalApi.Models;
using StackExchange.API.Helpers;

namespace StackExchange.API.Clients;

public interface ITagClient
{
    IEnumerable<ResponseData<Tags>> GetTags(StackExchangeQueryObject query);
}