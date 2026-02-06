using Gooios.BuildingBlocks.Domain.Seedwork;
using Gooios.BuildingBlocks.Seedwork;

namespace Gooios.BuildingBlocks.Application.Transaction;

public class TransactionContext : DisposableObject
{
    private readonly IDbUnitOfWork? _dbUnitOfWork;
    private bool _isEnableTransaction = false;
    private bool _disposed = false;

    public TransactionContext(IDbUnitOfWork dbUnitOfWork, bool needTransaction = false, System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.Serializable)
    {
        if (needTransaction)
        {
            _dbUnitOfWork = dbUnitOfWork;
            _dbUnitOfWork.BeginTransaction(isolationLevel);

            _isEnableTransaction = true;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (_isEnableTransaction)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbUnitOfWork?.Dispose();
                    _disposed = true;
                }
            }
        }
    }

    protected override ValueTask DisposeAsyncCore()
    {
        if (!_isEnableTransaction || _disposed) return ValueTask.CompletedTask;

        _dbUnitOfWork?.Dispose();
        _disposed = true;

        return ValueTask.CompletedTask;
    }
}