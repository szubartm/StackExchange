using StackExchange.API.Entities;
using StackExchange.API.Helpers;
using StackExchange.API.Models.Api;

namespace StackExchange.API.Clients;

public interface ITagClient
{
    IEnumerable<ResponseData<Tags>> GetTags(StackExchangeQueryObject query);
}