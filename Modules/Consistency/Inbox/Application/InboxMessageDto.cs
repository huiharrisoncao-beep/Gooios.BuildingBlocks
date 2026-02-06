namespace Gooios.BuildingBlocks.Modules.Consistency.Inbox.Application;

public record InboxMessageDto(Guid Id, string Type, string Data, DateTime OccurredOn, DateTime? ProcessedDate);
