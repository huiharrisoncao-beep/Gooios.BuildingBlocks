using Gooios.BuildingBlocks.Application.Contracts;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Domain;
using Gooios.BuildingBlocks.Modules.EventBus;
using Gooios.BuildingBlocks.Modules.EventBus.Events;
using Serilog;
using System.Reflection;
using System.Text.Json;

namespace Gooios.BuildingBlocks.Modules.Consistency.Outbox.Application;

/// <summary>
/// Processing outbox command
/// </summary>
/// <param name="OutboxMessageSchemaName">e.g. UserRoleOutboxMessage</param>
/// <param name="IntegrationEventAssembleName">e.g. AFI.Common.IntegrationEvents</param>
public record ProcessingOutboxCommand(string OutboxMessageSchemaName, string IntegrationEventAssembleName, IEnumerable<OutboxMessage> MessagesToProcess) : ICommand<bool>
{
}

public class ProcessingOutboxCommandHandler : ICommandHandler<ProcessingOutboxCommand, bool>
{
    readonly IEventBus _eventBus;
    readonly IOutboxMessageRepository _outboxMessageRepository;
    readonly ILogger _logger;
    public ProcessingOutboxCommandHandler(IEventBus eventBus, IOutboxMessageRepository outboxMessageRepository, ILogger logger)
    {
        _logger = logger;
        _eventBus = eventBus;
        _outboxMessageRepository = outboxMessageRepository;
    }

    public async Task<bool> Handle(ProcessingOutboxCommand command, CancellationToken cancellationToken)
    {
        var messages = command.MessagesToProcess;

        _logger.Information($"---> messages.count: {messages.Count()}");

        foreach (var message in messages)
        {
            try
            {
                var messageAssembly = Assembly.Load(command.IntegrationEventAssembleName)
                    ?? throw new InvalidOperationException($"Assembly '{command.IntegrationEventAssembleName}' not found");
                var type = messageAssembly.GetType(message.Type)
                    ?? throw new InvalidOperationException($"Type '{message.Type}' not found in assembly");

                _logger.Information($"---> type: {type.FullName}");

                var request = JsonSerializer.Deserialize(message.Data, type!)!;

                _logger.Information($"---> request is IntegrationEvent: {request is IntegrationEvent}");

                if (request is IntegrationEvent evt)
                    await _eventBus.Publish(evt);

                _logger.Information($"---> before SetOutboxMessageStatus");

                await _outboxMessageRepository.SetOutboxMessageStatusAsync(command.OutboxMessageSchemaName, message.Id, OutboxMessageStatus.Successful);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                await _outboxMessageRepository.SetOutboxMessageStatusAsync(command.OutboxMessageSchemaName, message.Id, OutboxMessageStatus.Failed, e.Message);
                return false;
            }
        }

        _logger.Information($"---> end");

        return true;
    }
}