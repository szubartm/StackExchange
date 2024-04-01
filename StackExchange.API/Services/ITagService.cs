using StackExchange.API.Data.Entities;

namespace StackExchange.API.Services;

public interface ITagService
{
    void SetPercentageOfAllGivenTags(List<TagDto> response);
}