using Gooios.BuildingBlocks.Modules.EventBus.Events;

namespace Gooios.BuildingBlocks.Modules.EventBus;

public interface IEventBus
{
    Task Publish<T>(T @event)
           where T : IntegrationEvent;

    void Subscribe<T>(IIntegrationEventHandler<T> handler)
        where T : IntegrationEvent;

    void Unsubscribe<T>(IIntegrationEventHandler<T> handler)
        where T : IntegrationEvent;
}