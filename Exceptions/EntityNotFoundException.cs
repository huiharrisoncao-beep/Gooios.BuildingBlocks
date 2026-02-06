namespace Gooios.BuildingBlocks.Exceptions;

public class EntityNotFoundException : Exception
{
    public string? EntityType { get; }
    public object? EntityId { get; }

    public EntityNotFoundException()
        : base("The requested entity was not found.")
    { }

    public EntityNotFoundException(string entityType, object entityId)
        : base($"Entity '{entityType}' with ID '{entityId}' was not found.")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public EntityNotFoundException(string message)
        : base(message)
    { }

    public EntityNotFoundException(string message, Exception inner)
        : base(message, inner)
    { }

    // Generic factory method for type-safe usage
    public static EntityNotFoundException For<TEntity>(object id)
        => new(typeof(TEntity).Name, id);
}
