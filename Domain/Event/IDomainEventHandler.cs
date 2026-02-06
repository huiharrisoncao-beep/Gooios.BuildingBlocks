using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Domain;
using Gooios.BuildingBlocks.Modules.EventBus.Events;
using MediatR;
using System.Text.Json;

namespace Gooios.BuildingBlocks.Domain.Event;

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
       where TDomainEvent : class, IDomainEvent
{
}

public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : class, IDomainEvent
{
    protected readonly IOutboxMessageRepository _outboxMessageRepository;

    public DomainEventHandler(IOutboxMessageRepository outboxMessageRepository)
    {
        _outboxMessageRepository = outboxMessageRepository;
    }

    public abstract Task Handle(TDomainEvent @event, CancellationToken cancellationToken);

    protected virtual async Task PublishIntegrationEvent<TEvent>(TEvent integrationEvent) where TEvent : IntegrationEvent
    {
        var integrationEventJson = JsonSerializer.Serialize(integrationEvent);
        var outboxMessage = OutboxMessage.CreateInstance(Guid.NewGuid(), DateTime.Now, typeof(TEvent).FullName ?? "", integrationEventJson, integrationEvent.Initiator);
        await _outboxMessageRepository.AddAsync(outboxMessage);
    }
}