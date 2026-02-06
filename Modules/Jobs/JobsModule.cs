using Autofac;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Jobs;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Jobs;
using Microsoft.EntityFrameworkCore;

namespace Gooios.BuildingBlocks.Modules.Jobs;

public class JobsModule<TDbContext, TInboxProcessJob, TOutboxProcessJob> : Module
    where TDbContext : DbContext
    where TInboxProcessJob: ProcessInboxJob
    where TOutboxProcessJob: ProcessOutboxJob
{
    private readonly string _integrationEventAssemblyName;

    public JobsModule(string integrationEventAssemblyName)
    {
        _integrationEventAssemblyName = integrationEventAssemblyName;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule(new InboxModule<TDbContext, TInboxProcessJob>());
        builder.RegisterModule(new OutboxModule<TDbContext, TOutboxProcessJob>(_integrationEventAssemblyName));
    }
}