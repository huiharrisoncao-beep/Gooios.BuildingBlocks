using Autofac;
using Module = Autofac.Module;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Application;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Domain;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Jobs;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Repository;
using Microsoft.EntityFrameworkCore;

namespace Gooios.BuildingBlocks.Modules.Consistency.Outbox;

internal class OutboxModule<TDbContext, TProcessInboxJob> : Module
    where TDbContext : DbContext
    where TProcessInboxJob : ProcessOutboxJob
{
    private readonly string _integrationEventAssemblyName;

    public OutboxModule(string integrationEventAssemblyName)
    {
        _integrationEventAssemblyName = integrationEventAssemblyName;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ModuleConfiguration>()
               .WithParameter("integrationEventAssemblyName", _integrationEventAssemblyName)
               .SingleInstance();

        builder.RegisterType<ProcessingOutboxCommandHandler>()
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterType<ObtainOutboxMessagesToProcessQueryHandler>()
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterType<OutboxMessageRepository<TDbContext>>()
              .As(typeof(IOutboxMessageRepository))
              .InstancePerLifetimeScope();

        builder.RegisterType<TProcessInboxJob>()
               .InstancePerDependency();
    }
}