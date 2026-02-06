using Gooios.BuildingBlocks.Modules.EventBus.Events;
using System.Collections.Concurrent;

namespace Gooios.BuildingBlocks.Modules.EventBus;

public sealed class InMemoryEventBus
{
    static InMemoryEventBus()
    {
    }

    private InMemoryEventBus()
    {
        _handlersDictionary = new ConcurrentDictionary<string, ConcurrentBag<IIntegrationEventHandler>>();
    }

    public static InMemoryEventBus Instance { get; } = new InMemoryEventBus();

    private readonly IDictionary<string, ConcurrentBag<IIntegrationEventHandler>> _handlersDictionary;

    public void Subscribe<T>(IIntegrationEventHandler<T> handler)
        where T : IntegrationEvent
    {
        var eventType = typeof(T).FullName;
        if (eventType != null)
        {
            if (_handlersDictionary.ContainsKey(eventType))
            {
                var handlers = _handlersDictionary[eventType];
                handlers.Add(handler);
            }
            else
            {
                _handlersDictionary.Add(eventType, new ConcurrentBag<IIntegrationEventHandler> { handler });
            }
        }
    }

    public void UnSubscribe<T>(IIntegrationEventHandler<T> handler)
        where T : IntegrationEvent
    {
        var eventType = typeof(T).FullName;
        if (eventType != null)
        {
            if (_handlersDictionary.ContainsKey(eventType))
            {
                var handlers = _handlersDictionary[eventType];
                // ConcurrentBag<T> does not have a Remove method.
                // To "remove", create a new bag without the handler and replace the entry.
                var newHandlers = new ConcurrentBag<IIntegrationEventHandler>(
                    handlers.Where(h => !object.ReferenceEquals(h, handler))
                );
                _handlersDictionary[eventType] = newHandlers;
            }
        }
    }

    public void ClearUpSubscriptions()
    {
        _handlersDictionary.Clear();
    }

    public async Task Publish<T>(T @event)
        where T : IntegrationEvent
    {
        var eventType = @event.GetType().FullName;

        if (eventType == null)
            return;

        if (!_handlersDictionary.ContainsKey(eventType)) 
            return;

        if (_handlersDictionary.TryGetValue(eventType, out var integrationEventHandlers))
        {
            foreach (var integrationEventHandler in integrationEventHandlers)
            {
                var method = integrationEventHandler.GetType().GetMethod("Handle");
                if (method?.Invoke(integrationEventHandler, [ @event ]) is Task task)
                    await task;
            }
        }

        await Task.CompletedTask;
    }
}