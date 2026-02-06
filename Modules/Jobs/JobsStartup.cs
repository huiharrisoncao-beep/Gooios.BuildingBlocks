using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Jobs;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Jobs;
using Quartz;
using Quartz.Impl;
using Serilog;
using System.Collections.Specialized;

namespace Gooios.BuildingBlocks.Modules.Jobs;

public static class JobsStartup
{
    public static void Initialize<TInboxProcessJob, TOutboxProcessJob>(ILogger logger, long? internalProcessingPoolingInterval = null, string jobName = "Gooios.BuildingBlocks.Infrastructure.Jobs", string jobValue = "GooiosJob")
        where TInboxProcessJob : ProcessInboxJob
        where TOutboxProcessJob : ProcessOutboxJob
    {
        logger?.Information("Quartz starting...");

        var schedulerConfiguration = new NameValueCollection();
        schedulerConfiguration.Add(jobName, jobValue);

        ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
        IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

        scheduler.Start().GetAwaiter().GetResult();

        var processInboxJob = JobBuilder.Create<TInboxProcessJob>().Build();
        ITrigger trigger;
        if (internalProcessingPoolingInterval.HasValue)
        {
            trigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithSimpleSchedule(x =>
                        x.WithInterval(TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                            .RepeatForever())
                    .Build();
        }
        else
        {
            trigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/3 * * * * ?")
                    .Build();
        }

        scheduler
            .ScheduleJob(processInboxJob, trigger)
            .GetAwaiter().GetResult();

        var processOutboxJob = JobBuilder.Create<TOutboxProcessJob>().Build();

        ITrigger processInboxTrigger;
        if (internalProcessingPoolingInterval.HasValue)
        {
            processInboxTrigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithSimpleSchedule(x =>
                        x.WithInterval(TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                            .RepeatForever())
                    .Build();
        }
        else
        {
            processInboxTrigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/3 * * * * ?")
                    .Build();
        }

        scheduler
            .ScheduleJob(processOutboxJob, processInboxTrigger)
            .GetAwaiter().GetResult();

        logger?.Information("Quartz started.");
    }
}