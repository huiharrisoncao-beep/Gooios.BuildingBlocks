namespace Gooios.BuildingBlocks.Exceptions;

public class CurrentUserNotFoundException : Exception
{
    private const string DefaultMessage = "The current user context could not be found.";

    public CurrentUserNotFoundException()
        : base(DefaultMessage)
    { }

    public CurrentUserNotFoundException(string message)
        : base(message)
    { }

    public CurrentUserNotFoundException(string message, Exception inner)
        : base(message, inner)
    { }
}
