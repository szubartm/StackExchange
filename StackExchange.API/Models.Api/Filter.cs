using StackExchange.API.Enums;

namespace StackExchange.API.Entities;

public class Filter(Order order)
{
    public Order? Order { get; } = order;
    public string Site => "stackoverflow";
}