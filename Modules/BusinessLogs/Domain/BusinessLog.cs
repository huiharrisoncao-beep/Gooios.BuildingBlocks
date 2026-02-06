using Gooios.BuildingBlocks.Domain.Seedwork;

namespace Gooios.BuildingBlocks.Modules.BusinessLogs.Domain;

public class BusinessLog : AggregateRoot<string>
{
    private BusinessLog() { }

    private BusinessLog(string operateName,
                        string schemaName,
                        string schemaKeyValue,
                        string oldValue,
                        string newValue,
                        string operatorName)
    {
        var now = DateTime.UtcNow;
        OperateName = operateName;
        SchemaName = schemaName;
        SchemaKeyValue = schemaKeyValue;
        OldValue = oldValue;
        NewValue = newValue;
        Status = OperationLogStatus.UnProcess;
        CreatedBy = operatorName;
        UpdatedBy = operatorName;
        CreatedOn = now;
        UpdatedOn = now;
    }

    public string? OperateName { get; set; }

    public string? SchemaName { get; set; }

    public string? SchemaKeyValue { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public OperationLogStatus? Status { get; set; }

    public void GenerateId()
    {
        Id = Guid.NewGuid().ToString();
    }

    public void SetCollected(OperationLogStatus status)
    {
        Status = status;
        UpdatedBy = "Job";
        UpdatedOn = DateTime.UtcNow;
    }

    public static BusinessLog CreateInstance(string operateName,
                                             string schemaName,
                                             string schemaKeyValue,
                                             string oldValue,
                                             string newValue,
                                             string operatorName)
    {
        var result = new BusinessLog(operateName, schemaName, schemaKeyValue, oldValue, newValue, operatorName);
        result.GenerateId();
        return result;
    }
}

public enum OperationLogStatus
{
    Init = 0,
    UnProcess = 1,
    Processing = 3,
    Processed = 5
}

public enum OperationType
{
    ADD,
    UPDATE,
    SUBMIT,
    APPROVE,
    DELETE,
    BUY,
    EXCHANGE
}

