using Autofac;

namespace Gooios.BuildingBlocks;

/// <summary>
/// Only can be used in microservice, mutiple module application need to redefine CompositionRoot/ProcessInboxJob/ProcessOutboxJob
/// </summary>
public class CompositionRoot
{
    private static IContainer? _container;

    internal static void SetContainer(IContainer container)
    {
        _container = container;
    }

    internal static ILifetimeScope BeginLifetimeScope()
    {
        return _container!.BeginLifetimeScope();
    }

    public static IContainer? Container
    {
        get
        {
            return _container;
        }
    }
}