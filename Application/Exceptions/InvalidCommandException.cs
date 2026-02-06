namespace Gooios.BuildingBlocks.Application.Exceptions;

public class InvalidCommandException : System.Exception
{
    public IReadOnlyList<string> Errors { get; }

    public InvalidCommandException(List<string> errors)
        : base($"Command validation failed: {string.Join(", ", errors)}")
    {
        Errors = errors.AsReadOnly();
    }
}