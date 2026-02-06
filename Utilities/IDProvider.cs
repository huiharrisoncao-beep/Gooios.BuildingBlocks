using Snowflake.Core;

namespace Gooios.BuildingBlocks.Utilities;

public class IDProvider
{
    private static readonly Lazy<IdWorker> _idWorker = new(() =>
    {
        var workerId = 1L; // Should be from configuration
        var datacenterId = 1L; // Should be from configuration
        return new IdWorker(workerId, datacenterId);
    });

    public static IdWorker IdWorker => _idWorker.Value;

    // Alternative: Configurable initialization
    private static IdWorker? _configuredWorker;
    private static readonly object _lock = new();

    public static void Initialize(long workerId, long datacenterId)
    {
        if (_configuredWorker != null)
            throw new InvalidOperationException("IdProvider has already been initialized.");

        lock (_lock)
        {
            _configuredWorker ??= new IdWorker(workerId, datacenterId);
        }
    }


}