using Newtonsoft.Json;

namespace StackExchange.API.ExternalApi.Models;

public class ResponseData<T>
{
    [JsonProperty("backoff")] public int? Backoff { get; set; }
    [JsonProperty("error_id")] public int? ErrorId { get; set; }
    [JsonProperty("error_message")] public string? ErrorMessage { get; set; }
    [JsonProperty("error_name")] public string? ErrorName { get; set; }
    [JsonProperty("has_more")] public bool HasMore { get; set; }
    [JsonProperty("items")] public T[] Items { get; set; }
    [JsonProperty("quota_max")] public long QuotaMax { get; set; }
    [JsonProperty("quota_remaining")] public long QuotaRemaining { get; set; }
}