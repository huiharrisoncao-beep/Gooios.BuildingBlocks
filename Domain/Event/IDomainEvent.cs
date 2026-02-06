using Gooios.BuildingBlocks.Domain.Seedwork;
using Gooios.BuildingBlocks.Seedwork;
using MediatR;

namespace Gooios.BuildingBlocks.Domain.Event;

public interface IDomainEvent : IEvent, INotification
{
    IEntity Source { get; }
}

public abstract class DomainEvent : IDomainEvent
{
    public DomainEvent(IEntity source, Guid? correlationId = null)
    {
        ID = Guid.NewGuid();
        Source = source;
        OccurredAt = DateTime.Now;
        CorrelationId = correlationId ?? Guid.NewGuid();
    }

    public Guid ID { get; private set; }

    public IEntity Source { get; private set; }
    
    public DateTimeOffset OccurredAt { get; private set; }

    public Guid? CorrelationId { get; private set; }
}