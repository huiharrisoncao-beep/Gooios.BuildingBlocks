using Gooios.BuildingBlocks.Modules.EventBus.Events;
using Gooios.BuildingBlocks.Seedwork;

namespace Gooios.BuildingBlocks.Modules.EventBus;

public class InMemoryEventBusClient : DisposableObject, IEventBus
{
    public InMemoryEventBusClient() { }

    protected override void DisposeManagedResources()
    {
        InMemoryEventBus.Instance.ClearUpSubscriptions();
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        InMemoryEventBus.Instance.ClearUpSubscriptions();
    }

    public async Task Publish<T>(T @event)
        where T : IntegrationEvent
    {
        await InMemoryEventBus.Instance.Publish(@event);
    }

    public void Subscribe<T>(IIntegrationEventHandler<T> handler)
        where T : IntegrationEvent
    {
        InMemoryEventBus.Instance.Subscribe(handler);
    }

    public void Unsubscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
    {
        InMemoryEventBus.Instance.UnSubscribe(handler);
    }
}