namespace Gooios.BuildingBlocks.Exceptions;

public class ArgumentInvalidException : ArgumentException
{
    public ArgumentInvalidException(string paramName)
       : base($"Argument '{paramName}' is invalid.", paramName)
    { }

    public ArgumentInvalidException(string paramName, string message)
        : base(message, paramName)
    { }

    public ArgumentInvalidException(string paramName, string message, Exception inner)
        : base(message, inner)
    {
        // Note: ArgumentException(string, string, Exception) doesn't exist
        // Consider storing paramName separately
    }
}