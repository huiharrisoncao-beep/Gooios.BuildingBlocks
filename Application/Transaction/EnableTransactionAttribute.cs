namespace Gooios.BuildingBlocks.Application.Transaction;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
public class EnableTransactionAttribute : Attribute
{
}

public class CanNotGetAttributeException : Exception
{
    public int ErrorCode { get; set; }

    public CanNotGetAttributeException(int errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}