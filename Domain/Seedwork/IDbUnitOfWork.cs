namespace Gooios.BuildingBlocks.Domain.Seedwork;

public interface IDbUnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
{
    Task BeginTransactionAsync(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.Serializable, CancellationToken cancellationToken = default);

    void BeginTransaction(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.Serializable, CancellationToken cancellationToken = default);
}