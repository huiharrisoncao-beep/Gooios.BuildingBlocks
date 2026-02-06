using Autofac;
using AutoMapper;
using Gooios.BuildingBlocks.Modules.BusinessLogs;
using Gooios.BuildingBlocks.Infrastructure.Repository;
using Gooios.BuildingBlocks.Modules.BusinessLogs.Configuration;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Configuration;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Configuration;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Jobs;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Jobs;
using System.Reflection;
using Gooios.BuildingBlocks.Modules.Decorators;
using Gooios.BuildingBlocks.Modules.EventBus;
using Gooios.BuildingBlocks.Modules.Jobs;
using Gooios.BuildingBlocks.Modules.Log;
using Gooios.BuildingBlocks.Modules.Mediations;

namespace Gooios.BuildingBlocks;

public class BuildingBlockModule<TDbContext, TInboxProcessJob, TOutboxProcessJob> : Autofac.Module
    where TDbContext : DbContext
    where TInboxProcessJob : ProcessInboxJob
    where TOutboxProcessJob : ProcessOutboxJob
{
    private readonly string _integrationEventAssemblyName;
    private readonly IDbConfiguration _dbConfiguration;
    private readonly ILogger _logger;
    private readonly IEventBus? _eventsBus;
    private readonly Assembly _mediatRRegisterAssembly;

    public BuildingBlockModule(string integrationEventAssemblyName, 
        IDbConfiguration dbConfiguration, 
        ILogger logger, 
        IEventBus? eventBus,
        Assembly mediatRRegisterAssembly)
    {
        _integrationEventAssemblyName = integrationEventAssemblyName;
        _dbConfiguration = dbConfiguration;
        _logger = logger;
        _eventsBus = eventBus;
        _mediatRRegisterAssembly = mediatRRegisterAssembly;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BusinessLogsProfile>();
            cfg.AddProfile<InboxMessageProfile>();
            cfg.AddProfile<OutboxMessageProfile>();
        }));

        builder.RegisterModule(new BusinessLogsModule<TDbContext>());
        builder.RegisterModule(new JobsModule<TDbContext, TInboxProcessJob, TOutboxProcessJob>(_integrationEventAssemblyName));
        builder.RegisterModule(new DecoratorModule<TDbContext>(_dbConfiguration));
        builder.RegisterModule(new LoggingModule(_logger));
        builder.RegisterModule(new MediatorModule(_mediatRRegisterAssembly));
        builder.RegisterModule(new EventsBusModule(_eventsBus));
    }
}