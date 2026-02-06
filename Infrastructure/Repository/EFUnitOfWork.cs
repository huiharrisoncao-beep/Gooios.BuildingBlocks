using Gooios.BuildingBlocks.Domain.Event;
using Gooios.BuildingBlocks.Domain.Seedwork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Gooios.BuildingBlocks.Infrastructure.Repository;

public class EFUnitOfWork<TDbContext> : IEFUnitOfWork<TDbContext> where TDbContext : DbContext
{
    readonly IDbContextProvider<TDbContext> _provider;
    readonly IMediator _mediator;

    bool _disposed = false;
    TDbContext? _dbContext = null;
    IDbContextTransaction? _transaction = null;

    public TDbContext DatabaseContext => _dbContext ?? (_dbContext = _provider.GetDbContext());

    public EFUnitOfWork(IDbContextProvider<TDbContext> provider, IMediator mediator)
    {
        _provider = provider;
        _mediator = mediator;
    }

    /// <summary>
    /// Start a transaction async, can set isolation level
    /// </summary>
    public async Task BeginTransactionAsync(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.Serializable, CancellationToken cancellationToken = default)
    {
        if(_transaction != null)
            throw new InvalidOperationException("Transaction already started");

        _transaction = await DatabaseContext.Database.BeginTransactionAsync(isolationLevel).ConfigureAwait(false);
    }

    /// <summary>
    /// Start a transaction, can set isolation level
    /// </summary>
    public void BeginTransaction(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.Serializable, CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction already started");

        _transaction = DatabaseContext.Database.BeginTransaction(isolationLevel);
    }

    public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if(_disposed)
            throw new ObjectDisposedException(nameof(EFUnitOfWork<TDbContext>));

        try
        {
            await DispatchDomainEvents();
            await DatabaseContext.SaveChangesAsync();

            if (_transaction != null)
                await _transaction!.CommitAsync();
        }
        catch
        {
            await RollbackAsync().ConfigureAwait(false);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync().ConfigureAwait(false);
        }
    }

    protected async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) return;

        await _transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task DispatchDomainEvents(CancellationToken cancellationToken = default)
    {
        var domainEvents = GetDomainEventsAndClear().ToList();

        while (domainEvents.Any())
        {
            var currentEvents = domainEvents.ToList();
            foreach (var e in currentEvents)
            {
                await _mediator.Publish(e);
            }

            var newEvents = GetDomainEventsAndClear();
            if (newEvents.Any())
                domainEvents.AddRange(newEvents);
        }
    }

    private IEnumerable<IDomainEvent> GetDomainEventsAndClear()
    {
        var changeTracker = _provider.GetDbContext().ChangeTracker;
        if (changeTracker == null) return Enumerable.Empty<IDomainEvent>();

        changeTracker.DetectChanges();

        var entries = changeTracker.Entries<IEntity>().Where(e => e.Entity.DomainEvents?.Any() == true).ToList();
        if (entries == null)
            return Enumerable.Empty<IDomainEvent>();

        var events = entries.SelectMany(e => e.Entity.DomainEvents!).ToList();
        entries.ForEach(e => e.Entity.ClearDomainEvents());
        return events;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Here clear up the delegated resources

            DisposeTransactionAsync().GetAwaiter().GetResult();
            DisposeDbContext().GetAwaiter().GetResult();
        }

        // Here clear up the Non-delegated resources

        _disposed = true;
    }

    protected virtual async ValueTask DisposeCoreAsync()
    {
        if (_disposed) return;

        await DisposeTransactionAsync();
        await DisposeDbContext();

        _disposed = true;
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeCoreAsync().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction == null) return;

        await _transaction.DisposeAsync().ConfigureAwait(false);
        _transaction = null;
    }

    private async Task DisposeDbContext()
    {
        if (_dbContext == null) return;

        await _dbContext.DisposeAsync();
        _dbContext = null;
    }

    ~EFUnitOfWork() => Dispose(false);
}