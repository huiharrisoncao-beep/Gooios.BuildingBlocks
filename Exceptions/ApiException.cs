namespace Gooios.BuildingBlocks.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; }
    public string? ErrorCode { get; }

    public ApiException(string message, int statusCode = 500, string? errorCode = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public ApiException(string message, Exception inner, int statusCode = 500)
        : base(message, inner)
    {
        StatusCode = statusCode;
    }
}