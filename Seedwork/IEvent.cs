namespace Gooios.BuildingBlocks.Seedwork;

/// <summary>
/// Represents a base interface for all events in the system.
/// </summary>
public interface IEvent
{
    /// <summary>
    /// Gets the unique identifier of the event.
    /// </summary>
    Guid ID { get; }

    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    DateTimeOffset OccurredAt { get; }

    /// <summary>
    /// Gets the correlation ID for tracing related events.
    /// </summary>
    Guid? CorrelationId { get; }

    /// <summary>
    /// Gets the name/type of the event for serialization.
    /// </summary>
    string EventType => GetType().Name;
}