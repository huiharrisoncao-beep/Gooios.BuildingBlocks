using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Domain;
using MediatR;
using System.Text.Json;

namespace Gooios.BuildingBlocks.Modules.EventBus.Events;

public interface IIntegrationEventHandler { }

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler 
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}

public abstract class IntegrationEventHandler<T> : INotificationHandler<T>
    where T : IntegrationEvent
{
    protected IOutboxMessageRepository _outboxMessageRepository;

    public IntegrationEventHandler(IOutboxMessageRepository outboxMessageRepository)
    {
        _outboxMessageRepository = outboxMessageRepository;
    }

    protected virtual async Task PublishIntegrationEvent<TEvent>(TEvent integrationEvent) where TEvent : IntegrationEvent
    {
        var integrationEventJson = JsonSerializer.Serialize(integrationEvent);
        var outboxMessage = OutboxMessage.CreateInstance(Guid.NewGuid(), DateTime.Now, typeof(TEvent).FullName ?? "", integrationEventJson, integrationEvent.Initiator);
        await _outboxMessageRepository.AddAsync(outboxMessage);
    }

    public abstract Task Handle(T notification, CancellationToken cancellationToken);
}