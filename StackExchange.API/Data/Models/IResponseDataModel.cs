namespace StackExchange.API.Data.Models;

public interface IResponseDataModel<T> : IResponseModel
{
    public T Data { get; set; }
}