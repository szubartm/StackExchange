using System.ComponentModel;

namespace StackExchange.API.Helpers;

public class StackExchangeQueryObject
{
    [DefaultValue(1000)] public int NumberOfExpectedTags { get; set; }

    [DefaultValue("name")] public string SortBy { get; set; }

    [DefaultValue("desc")] public string Order { get; set; }

    [DefaultValue(1)] public int PageNumber { get; set; }

    [DefaultValue(100)] public int PageSize { get; set; }
}