using StackExchange.API.Data.Entities;

namespace StackExchange.Api.UnitTests.Helpers;

public class DataHelper
{
    public static List<TagDto> GetFakeTagsList()
    {
        return
        [
            new TagDto
                { Id = 1, Name = "test1", HasSynonyms = true, Count = 100, IsRequired = false, IsModeratorOnly = true },

            new TagDto
            {
                Id = 2, Name = "test2", HasSynonyms = false, Count = 230, IsRequired = false, IsModeratorOnly = true
            },

            new TagDto
                { Id = 3, Name = "test3", HasSynonyms = true, Count = 413, IsRequired = true, IsModeratorOnly = true },

            new TagDto
            {
                Id = 4, Name = "test4", HasSynonyms = false, Count = 11, IsRequired = false, IsModeratorOnly = false
            },

            new TagDto
                { Id = 5, Name = "test5", HasSynonyms = true, Count = 66, IsRequired = true, IsModeratorOnly = false }
        ];
    }
}