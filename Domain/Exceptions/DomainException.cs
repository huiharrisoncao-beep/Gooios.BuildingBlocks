namespace Gooios.BuildingBlocks.Domain.Exceptions;

public class DomainException : System.Exception
{
    public DomainException() { }

    public DomainException(string message) : base(message)
    { }

    public DomainException(string message, System.Exception inner) : base(message, inner)
    { }
}