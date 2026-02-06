using Gooios.BuildingBlocks.Application.Contracts;
using Gooios.BuildingBlocks.Application.Transaction;
using Gooios.BuildingBlocks.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Gooios.BuildingBlocks.Modules.Decorators;

internal class UnitOfWorkCommandHandlerWithResultDecorator<D, T, TResult> : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
    where D : DbContext
{
    private readonly ICommandHandler<T, TResult> _decorated;
    private readonly IEFUnitOfWork<D> _unitOfWork;

    /// <summary>
    /// if this application separate to micro services, then we can use CAP.
    /// </summary>
    /// <param name="decorated"></param>
    /// <param name="unitOfWork"></param>
    public UnitOfWorkCommandHandlerWithResultDecorator(
        ICommandHandler<T, TResult> decorated,
        IEFUnitOfWork<D> unitOfWork
        )
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        if (typeof(T).IsDefined(typeof(EnableTransactionAttribute), false))
        {
            using (var scope = new TransactionContext(_unitOfWork!, true))
            {
                var result = await _decorated.Handle(command, cancellationToken);
                await _unitOfWork.CommitAsync();
                return result;
            }
        }
        else
        {
            var result = await _decorated.Handle(command, cancellationToken);
            await _unitOfWork.CommitAsync();
            return result;
        }

    }
}