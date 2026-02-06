using Gooios.BuildingBlocks.Domain.Repository;

namespace Gooios.BuildingBlocks.Modules.Consistency.Outbox.Domain;

public interface IOutboxMessageRepository : IRepository<OutboxMessage, Guid>
{
    Task<IEnumerable<OutboxMessage>> ObtainOutboxMessagesToProcessAsync(string outboxMessageSchemaName);

    Task<int> SetOutboxMessageStatusAsync(string outboxMessageSchemaName, Guid id, OutboxMessageStatus status, string? errorMessage = null);
}