using Autofac;
using Module = Autofac.Module;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Application;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Domain;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Jobs;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Repository;
using Microsoft.EntityFrameworkCore;

namespace Gooios.BuildingBlocks.Modules.Consistency.Inbox;

internal class InboxModule<TDbContext, TProcessInboxJob> : Module
    where TDbContext : DbContext
    where TProcessInboxJob : ProcessInboxJob
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ProcessingInboxCommandHandler>()
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterType<ObtainInboxMessagesToProcessQueryHandler>()
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterType<InboxMessageRepository<TDbContext>>()
              .As(typeof(IInboxMessageRepository))
              .InstancePerLifetimeScope();

        builder.RegisterType<TProcessInboxJob>()
               .InstancePerDependency();

    }
}