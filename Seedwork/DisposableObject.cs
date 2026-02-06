
namespace Gooios.BuildingBlocks.Seedwork;

public abstract class DisposableObject : IDisposable, IAsyncDisposable
{
    private bool _disposed;

    /// <summary>
    /// Gets a value indicating whether this object has been disposed.
    /// </summary>
    protected bool IsDisposed => _disposed;

    /// <summary>
    /// Throws <see cref="ObjectDisposedException"/> if this object has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }

    #region Finalization Constructs

    /// <summary>
    /// Finalizes the object.
    /// </summary>
    ~DisposableObject()
    {
        Dispose(false);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Disposes the object.
    /// </summary>
    /// <param name="disposing">A <see cref="bool"/> value which indicates whether
    /// the object should be disposed explicitly.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            DisposeManagedResources();
        }

        DisposeUnmanagedResources();
        _disposed = true;
    }

    /// <summary>
    /// Override to dispose managed resources.
    /// </summary>
    protected virtual void DisposeManagedResources() { }

    /// <summary>
    /// Override to dispose unmanaged resources.
    /// </summary>
    protected virtual void DisposeUnmanagedResources() { }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await ValueTask.CompletedTask;
    }

    #endregion

    #region IDisposable and IAsyncDisposable Members

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or
    /// resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    #endregion
}