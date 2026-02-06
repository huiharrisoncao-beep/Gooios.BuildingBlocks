namespace Gooios.BuildingBlocks.Modules.Consistency.Outbox.Application;

public record OutboxMessageDto(Guid Id, string Type, string Data, DateTime OccurredOn, DateTime? ProcessedDate);
