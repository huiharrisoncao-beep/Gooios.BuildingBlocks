using Gooios.BuildingBlocks.Domain.Seedwork;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Domain;
using Gooios.BuildingBlocks.Modules.EventBus.Events;
using Gooios.BuildingBlocks.Seedwork;
using System.Text.Json;

namespace Gooios.BuildingBlocks.Application.Contracts;

public record Command<T> : ICommand<T>
{
    public Guid Id { get; }

    public Command()
    {
        Id = Guid.NewGuid();
    }

    public Command(Guid id)
    {
        Id = id;
    }
}

public abstract class CommandHandler<TCommand, TResponse> : DisposableObject, ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    protected readonly IOutboxMessageRepository? _outboxMessageRepository;

    public CommandHandler() { }

    public CommandHandler(IOutboxMessageRepository outboxMessageRepository)
    {
        _outboxMessageRepository = outboxMessageRepository;
    }

    public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);

    protected async Task PublishIntegrationEvent<TEvent>(TEvent integrationEvent) 
         where TEvent:IntegrationEvent
    {
        if (_outboxMessageRepository == null)
        {
            throw new InvalidOperationException("OutboxMessageRepository is not initialized.");
        }

        var integrationEventJson = JsonSerializer.Serialize(integrationEvent);
        var outboxMessage = OutboxMessage.CreateInstance(Guid.NewGuid(), DateTime.Now, typeof(TEvent).FullName ?? "", integrationEventJson, integrationEvent.Initiator);
        await _outboxMessageRepository!.AddAsync(outboxMessage);
    }

}