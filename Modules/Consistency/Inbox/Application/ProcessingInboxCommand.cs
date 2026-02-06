using Gooios.BuildingBlocks.Application.Contracts;
using Gooios.BuildingBlocks.Application.Transaction;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Domain;
using MediatR;
using Serilog;
using System.Text.Json;

namespace Gooios.BuildingBlocks.Modules.Consistency.Inbox.Application;

[EnableTransaction]
public record ProcessingInboxCommand(string InboxMessageSchemaName, IEnumerable<InboxMessage> MessagesToProcess) : ICommand<bool>
{
}

public class ProcessingInboxCommandHandler : ICommandHandler<ProcessingInboxCommand, bool>
{
    readonly IInboxMessageRepository _inboxMessageRepository;
    readonly IMediator _mediator;
    readonly ILogger _logger;

    public ProcessingInboxCommandHandler(ILogger logger, IMediator mediator, IInboxMessageRepository inboxMessageRepository)
    {
        _mediator = mediator;
        _inboxMessageRepository = inboxMessageRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(ProcessingInboxCommand command, CancellationToken cancellationToken)
    {
        var messages = command.MessagesToProcess;

        _logger.Information($"---> messages.count: {messages.Count()}");

        foreach (var message in messages)
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(o => o.GetName().Name!.EndsWith("IntegrationEvents")).ToList();

                _logger.Information($"---> assemblies.count: {assemblies.Count()}");

                Type? type = null;
                foreach (var assembly in assemblies)
                {
                    type = assembly.GetTypes().FirstOrDefault(o => !string.IsNullOrEmpty(o.FullName) && o.FullName.Contains(message.Type));
                    if (type != null) break;
                }

                _logger.Information($"---> type: {type?.FullName}");

                var request = JsonSerializer.Deserialize(message.Data, type!)!;

                _logger.Information($"---> request type name: {request?.GetType().FullName}");

                await _mediator!.Publish((INotification)request!, cancellationToken);

                _logger.Information($"---> before SetInboxMessageStatus");

                await _inboxMessageRepository.SetInboxMessageStatusAsync(command.InboxMessageSchemaName, message.Id, InboxMessageStatus.Successful);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                await _inboxMessageRepository.SetInboxMessageStatusAsync(command.InboxMessageSchemaName, message.Id, InboxMessageStatus.Failed, e.Message);
                return false;
            }
        }

        _logger.Information($"---> end");

        return true;
    }
}