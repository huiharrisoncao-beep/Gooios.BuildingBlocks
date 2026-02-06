using Gooios.BuildingBlocks.Seedwork;
using MediatR;

namespace Gooios.BuildingBlocks.Modules.EventBus.Events;

public class IntegrationEvent : IEvent, INotification
{
    public IntegrationEvent(string initiator, Guid? correlationId)
    {
        ID = Guid.NewGuid();
        TimeStamp = DateTime.UtcNow;
        CorrelationId = correlationId ?? Guid.NewGuid();
        Initiator = initiator;
    }

    public Guid ID { get; private set; }

    public Guid? CorrelationId { get; set; }

    public DateTime TimeStamp { get; private set; }

    /// <summary>
    /// The initiator/actor of the event
    /// </summary>
    public string Initiator { get; set; }

    public DateTimeOffset OccurredAt { get; private set; }
}