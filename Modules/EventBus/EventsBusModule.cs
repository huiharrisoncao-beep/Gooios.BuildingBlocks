using Autofac;

namespace Gooios.BuildingBlocks.Modules.EventBus;

internal class EventsBusModule : Module
{
    private readonly IEventBus? _eventsBus;

    public EventsBusModule(IEventBus? eventsBus)
    {
        _eventsBus = eventsBus;
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (_eventsBus != null)
        {
            builder.RegisterInstance(_eventsBus).SingleInstance();
        }
        else
        {
            builder.RegisterType<InMemoryEventBusClient>()
                   .As<IEventBus>()
                   .SingleInstance();
        }
    }
}