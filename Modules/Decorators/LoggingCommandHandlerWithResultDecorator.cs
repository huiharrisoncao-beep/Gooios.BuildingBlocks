using Gooios.BuildingBlocks.Application.Contracts;
using MediatR;
using Serilog;
using System.Text.Json;

namespace Gooios.BuildingBlocks.Modules.Decorators;

internal class LoggingCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult>
        where T : ICommand<TResult>
{
    private readonly ILogger _logger;
    private readonly IRequestHandler<T, TResult> _decorated;

    public LoggingCommandHandlerWithResultDecorator(
        ILogger logger,
        ICommandHandler<T, TResult> decorated)
    {
        _logger = logger;
        _decorated = decorated;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information($"Start call {command.GetType().Name}. Command: {JsonSerializer.Serialize(command)}");

            var result = await _decorated.Handle(command, cancellationToken);

            _logger.Information($"End call  {command.GetType().Name}. Result: {JsonSerializer.Serialize(result)}");

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            throw;
        }
    }
}