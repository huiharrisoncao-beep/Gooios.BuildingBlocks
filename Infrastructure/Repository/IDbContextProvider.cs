using Microsoft.EntityFrameworkCore;

namespace Gooios.BuildingBlocks.Infrastructure.Repository;

public interface IDbContextProvider<TDbContext> : IDisposable, IAsyncDisposable where TDbContext : DbContext
{
    TDbContext GetDbContext();
}

public class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : DbContext
{
    private TDbContext? _dataContext;
    private IDbConfiguration _configuration;

    bool _disposed = false;

    public DbContextProvider(IDbConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TDbContext GetDbContext()
    {
        if (_dataContext != null) 
            return _dataContext;

        var connection = _configuration.ConnectionString;

        // Better: Use AutoDetect for MySQL version
        var options = _configuration.DatabaseType == DatabaseType.PostgreSql
            ? new DbContextOptionsBuilder<TDbContext>().UseNpgsql(connection, o => o.UseNetTopologySuite()).Options
            : new DbContextOptionsBuilder<TDbContext>().UseMySql(connection, ServerVersion.AutoDetect(connection)).Options;

        _dataContext = (TDbContext?)Activator.CreateInstance(typeof(TDbContext), options)
            ?? throw new InvalidOperationException($"Failed to create instance of {typeof(TDbContext).Name}");

        return _dataContext;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (!_disposed)
        {
            if (isDisposing)
            {
                if (_dataContext != null)
                {
                    _dataContext.Dispose();
                    _dataContext = null;
                }
                _disposed = true;
            }
        }
    }

    protected virtual async ValueTask DisposeCoreAsync()
    {
        if (_disposed) return;

        if (_dataContext != null)
        {
            _dataContext.Dispose();
            _dataContext = null;
        }

        _disposed = true;
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeCoreAsync().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    ~DbContextProvider()
    {
        Dispose(false);
    }
}