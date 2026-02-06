using Gooios.BuildingBlocks.Modules.BusinessLogs.Domain;
using MediatR;

namespace Gooios.BuildingBlocks.Modules.EventBus.Events;

public class LogIntegrationEvent : IntegrationEvent
{
    public string OperationType { get; set; }

    public string LogType { get; set; }

    public string Key { get; set; }

    public string OldSourceJson { get; set; }

    public string NewSourceJson { get; set; }

    public string Actor { get; set; }

    /// <summary>
    /// Initialize the object
    /// </summary>
    /// <param name="operationType">ADD/UPDATE/SUBMIT/APPROVE/DELETE</param>
    /// <param name="logType">schema name/ table name</param>
    /// <param name="key">id of the object</param>
    /// <param name="oldSourceJson">before changed</param>
    /// <param name="newSourceJson">after changed</param>
    /// <param name="actor">operator</param>
    public LogIntegrationEvent(string operationType, string logType, string key, string oldSourceJson, string newSourceJson, string actor, Guid? correlationId = null)
        : base(actor, correlationId)
    {
        OperationType = operationType;
        LogType = logType;
        Key = key;
        OldSourceJson = oldSourceJson;
        NewSourceJson = newSourceJson;
        Actor = actor;
    }
}

public class LogIntegrationEventHandler<T> : INotificationHandler<T>
    where T : LogIntegrationEvent
{
    readonly IBusinessLogRepository _operationLogRepository;
    public LogIntegrationEventHandler(IBusinessLogRepository operationLogRepository)
    {
        _operationLogRepository = operationLogRepository;
    }

    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        var log = BusinessLog.CreateInstance(notification.OperationType, notification.LogType, notification.Key, notification.OldSourceJson, notification.NewSourceJson, notification.Actor);
        await _operationLogRepository.AddAsync(log);
    }
}
