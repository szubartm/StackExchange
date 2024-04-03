﻿using Newtonsoft.Json;

namespace StackExchange.API.ExternalApi.Models;

public class Tags
{
    [JsonProperty("has_synonyms")] public bool HasSynonyms { get; set; }

    [JsonProperty("is_moderator_only")] public bool IsModeratorOnly { get; set; }

    [JsonProperty("is_required")] public bool IsRequired { get; set; }

    [JsonProperty("count")] public long Count { get; set; }

    [JsonProperty("name")] public string Name { get; set; }
    public decimal PercentageOfAllGivenTags { get; set; }
}