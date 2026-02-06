using MediatR;

namespace Gooios.BuildingBlocks.Application.Contracts;

public interface ICommand<out TResult> : IRequest<TResult>
{
    Guid Id => Guid.NewGuid();
}

public interface ICommand : IRequest
{
    Guid Id { get; }
}

public interface ICommandHandler<in TCommand> :
    IRequestHandler<TCommand>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResult> :
    IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
}