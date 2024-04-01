using System.ComponentModel;

namespace StackExchange.API.Helpers;

public class DbQueryObject
{
    [DefaultValue("null")] public string? SortBy { get; set; }

    [DefaultValue("desc")] public string Order { get; set; }

    [DefaultValue(1)] public int PageNumber { get; set; }

    [DefaultValue(25)] public int PageSize { get; set; }
}