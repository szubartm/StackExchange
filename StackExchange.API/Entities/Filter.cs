using StackExchange.API.Enums;

namespace StackExchange.API.Entities;

public class Filter
{
    public Filter()
    {
        Order = Enums.Order.Desc;
        Site = "stackoverflow";
    }

    public Order? Order { get; }
    public string Site { get; set; }
}