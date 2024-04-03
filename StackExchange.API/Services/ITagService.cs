using StackExchange.API.Data.Entities;

namespace StackExchange.API.Services;

public interface ITagService
{
    List<TagDto> SetPercentageOfAllGivenTags(List<TagDto> response);
}