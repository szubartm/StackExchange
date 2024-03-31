using StackExchange.API.Models.Api;

namespace StackExchange.API.Data.Entities;

public class TagDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool HasSynonyms { get; set; }
    public bool IsModeratorOnly { get; set; }
    public bool IsRequired { get; set; }
    public long Count { get; set; }

    public decimal PercentageOfAllGivenTags { get; set; }


    public static implicit operator TagDto(Tags tag)
    {
        return new TagDto
        {
            Name = tag.Name,
            HasSynonyms = tag.HasSynonyms,
            IsModeratorOnly = tag.IsModeratorOnly,
            IsRequired = tag.IsRequired,
            Count = tag.Count,
            PercentageOfAllGivenTags = tag.PercentageOfAllGivenTags
        };
    }
}