using Gooios.BuildingBlocks.Application.Contracts;

namespace Gooios.BuildingBlocks;

public interface IModuleAccessor
{
    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

    Task ExecuteCommandAsync(ICommand command);

    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}