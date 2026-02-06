using Gooios.BuildingBlocks.Application.Contracts;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Domain;

namespace Gooios.BuildingBlocks.Modules.Consistency.Outbox.Application;

public record ObtainOutboxMessagesToProcessQuery(string InboxMessageSchemaName) : Query<IEnumerable<OutboxMessage>>;

public class ObtainOutboxMessagesToProcessQueryHandler : QueryHandler<ObtainOutboxMessagesToProcessQuery, IEnumerable<OutboxMessage>>
{
    readonly IOutboxMessageRepository _inboxMessageRepository;

    public ObtainOutboxMessagesToProcessQueryHandler(IOutboxMessageRepository inboxMessageRepository)
    {
        _inboxMessageRepository = inboxMessageRepository;
    }

    public override async Task<IEnumerable<OutboxMessage>> Handle(ObtainOutboxMessagesToProcessQuery request, CancellationToken cancellationToken)
    {
        var messages = (await _inboxMessageRepository.ObtainOutboxMessagesToProcessAsync(request.InboxMessageSchemaName)).ToList();
        return messages;
    }
}
