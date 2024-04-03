using StackExchange.API.CustomExceptions;
using StackExchange.API.Models.Api;

namespace StackExchange.API.Helpers;

internal static class ExtensionMethods
{
    internal static StackExchangeResponse<T> ValidateResponse<T>(this ResponseData<T> responseData)
    {
        var result = new StackExchangeResponse<T>();
        try
        {
            if (responseData.ErrorId is not null)
                throw new StackExchangeException(responseData.ErrorId.Value, responseData.ErrorName,
                    responseData.ErrorMessage);

            result.ResponseData = responseData;
            result.Success = true;
        }
        catch (Exception ex)
        {
            result.Success = false;
        }

        return result;
    }
}