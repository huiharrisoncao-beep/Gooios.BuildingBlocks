namespace Gooios.BuildingBlocks.Application.Exceptions;

public class AppServiceException : Exception
{
    public string? ErrorCode { get; protected set; }
    public DateTime OccurredAt { get; } = DateTime.Now;

    public AppServiceException() { }

    public AppServiceException(string message) : base(message) { }

    public AppServiceException(string message, Exception inner)
        : base(message, inner) { }
}