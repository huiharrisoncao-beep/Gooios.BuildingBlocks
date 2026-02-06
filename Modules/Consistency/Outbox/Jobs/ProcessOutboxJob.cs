using Autofac;
using MediatR;
using Quartz;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Application;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Repository;

namespace Gooios.BuildingBlocks.Modules.Consistency.Outbox.Jobs;

/// <summary>
/// Only can be used in microservice, mutiple module application need to redefine CompositionRoot/ProcessInboxJob/ProcessOutboxJob
/// </summary>
[DisallowConcurrentExecution]
public class ProcessOutboxJob : IJob
{
    public virtual async Task Execute(IJobExecutionContext context)
    {
        using (var scope = CompositionRoot.BeginLifetimeScope())
        {
            var moduleConfiguration = scope.Resolve<ModuleConfiguration>();
            var mediator = scope.Resolve<IMediator>();
            var messages = await mediator.Send(new ObtainOutboxMessagesToProcessQuery(OutboxMessageConfiguration.SCHEMA_NAME));

            await mediator.Send(new ProcessingOutboxCommand(OutboxMessageConfiguration.SCHEMA_NAME, moduleConfiguration.IntegrationEventAssemblyName, messages));
        }
    }
}