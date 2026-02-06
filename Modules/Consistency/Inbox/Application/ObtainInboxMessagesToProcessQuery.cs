using Gooios.BuildingBlocks.Application.Contracts;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Domain;

namespace Gooios.BuildingBlocks.Modules.Consistency.Inbox.Application;

public record ObtainInboxMessagesToProcessQuery(string InboxMessageSchemaName) : Query<IEnumerable<InboxMessage>>;

public class ObtainInboxMessagesToProcessQueryHandler : QueryHandler<ObtainInboxMessagesToProcessQuery, IEnumerable<InboxMessage>>
{
    readonly IInboxMessageRepository _inboxMessageRepository;

    public ObtainInboxMessagesToProcessQueryHandler(IInboxMessageRepository inboxMessageRepository)
    {
        _inboxMessageRepository = inboxMessageRepository;
    }

    public override async Task<IEnumerable<InboxMessage>> Handle(ObtainInboxMessagesToProcessQuery request, CancellationToken cancellationToken)
    {
        var messages = (await _inboxMessageRepository.ObtainInboxMessagesToProcessAsync(request.InboxMessageSchemaName)).ToList();
        return messages;
    }
}
