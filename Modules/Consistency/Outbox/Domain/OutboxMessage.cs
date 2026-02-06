using Gooios.BuildingBlocks.Domain.Seedwork;

namespace Gooios.BuildingBlocks.Modules.Consistency.Outbox.Domain;

public class OutboxMessage : AggregateRoot<Guid>
{
    public DateTime OccurredOn { get; set; }
    public string Type { get; set; } = null!;
    public string Data { get; set; } = null!;
    public DateTime? ProcessedDate { get; set; }
    public OutboxMessageStatus Status { get; set; }

    public string? ErrorMessage { get; set; }

    private OutboxMessage(Guid id, DateTime occurredOn, string type, string data, string actor)
    {
        Id = id;
        OccurredOn = occurredOn;
        Type = type;
        Data = data;
        Status = OutboxMessageStatus.Init;
        CreatedBy = UpdatedBy = actor;
        CreatedOn = UpdatedOn = DateTime.Now;
    }

    private OutboxMessage() { }

    public static OutboxMessage CreateInstance(Guid id, DateTime occurredOn, string type, string data, string actor)
        => new OutboxMessage(id, occurredOn, type, data, actor);

}

public enum OutboxMessageStatus
{
    Init = 1,
    Processing = 3,
    Failed = 5,
    Successful = 8
}