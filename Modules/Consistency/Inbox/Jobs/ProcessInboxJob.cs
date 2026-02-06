using Autofac;
using MediatR;
using Quartz;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Application;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Repository;

namespace Gooios.BuildingBlocks.Modules.Consistency.Inbox.Jobs;

/// <summary>
/// Only can be used in microservice, mutiple module application need to redefine CompositionRoot/ProcessInboxJob/ProcessOutboxJob
/// </summary>
[DisallowConcurrentExecution]
public class ProcessInboxJob : IJob
{
    public virtual async Task Execute(IJobExecutionContext context)
    {
        using (var scope = CompositionRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            var messages = await mediator.Send(new ObtainInboxMessagesToProcessQuery(InboxMessageConfiguration.SCHEMA_NAME));

            await mediator.Send(new ProcessingInboxCommand(InboxMessageConfiguration.SCHEMA_NAME, messages));
        }
    }
}