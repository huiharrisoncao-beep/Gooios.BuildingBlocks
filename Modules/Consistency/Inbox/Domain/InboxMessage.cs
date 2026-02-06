using Gooios.BuildingBlocks.Domain.Seedwork;

namespace Gooios.BuildingBlocks.Modules.Consistency.Inbox.Domain;

public class InboxMessage : AggregateRoot<Guid>
{
    public DateTime OccurredOn { get; set; }
    public string Type { get; set; } = null!;
    public string Data { get; set; } = null!;
    public DateTime? ProcessedDate { get; set; }
    public InboxMessageStatus Status { get; set; }

    public string? ErrorMessage { get; set; }

    private InboxMessage(Guid id, DateTime occurredOn, string type, string data, string actor)
    {
        Id = id;
        OccurredOn = occurredOn;
        Type = type;
        Data = data;
        Status = InboxMessageStatus.Init;
        CreatedBy = UpdatedBy = actor;
        CreatedOn = UpdatedOn = DateTime.Now;
    }

    private InboxMessage() { }

    public static InboxMessage CreateInstance(Guid id, DateTime occurredOn, string type, string data, string actor)
        => new InboxMessage(id, occurredOn, type, data, actor);

    public void SetStatus(InboxMessageStatus status, string operatorUser, string? errorMessage = null)
    {
        Status = status;
        UpdatedOn = DateTime.Now;
        UpdatedBy = operatorUser;
        if (!string.IsNullOrEmpty(errorMessage))
            ErrorMessage = errorMessage;
    }
}

public enum InboxMessageStatus
{
    Init = 1,
    Processing = 3,
    Failed = 5,
    Successful = 8
}
