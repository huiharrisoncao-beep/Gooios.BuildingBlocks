using Gooios.BuildingBlocks.Domain.Repository;

namespace Gooios.BuildingBlocks.Modules.Consistency.Inbox.Domain;

public interface IInboxMessageRepository : IRepository<InboxMessage, Guid>
{
    Task<IEnumerable<InboxMessage>> ObtainInboxMessagesToProcessAsync(string inboxMessageSchemaName);

    Task<int> SetInboxMessageStatusAsync(string inboxMessageSchemaName, Guid id, InboxMessageStatus status, string? errorMessage = null);
}
