using CacheManager.Core;

namespace Gooios.BuildingBlocks.Extensions;

public static class CacheManagerExtension
{
    public static async Task<TCacheValue?> GetOrAddAsync<TCacheValue>(
        this ICacheManager<TCacheValue> cacheManager,
        string key,
        Func<string, Task<TCacheValue>> valueFactory,
        string? region = null,
        ExpirationMode expirationMode = ExpirationMode.None,
        TimeSpan? expiration = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(valueFactory);

        // Try to get existing value
        var result = region is not null
            ? cacheManager.Get<TCacheValue>(key, region)
            : cacheManager.Get<TCacheValue>(key);

        if (result is not null)
            return result;

        // Create new value
        result = await valueFactory(key);
        if (result is null)
            return result;

        // Add to cache
        if (region is not null)
            cacheManager.Add(key, result, region);
        else
            cacheManager.Add(key, result);

        // Set expiration
        if (expiration.HasValue && expirationMode is ExpirationMode.Sliding or ExpirationMode.Absolute)
        {
            if (region is not null)
                cacheManager.Expire(key, region, expirationMode, expiration.Value);
            else
                cacheManager.Expire(key, expirationMode, expiration.Value);
        }

        return result;
    }

    public static void Remove<TCacheValue>(this ICacheManager<TCacheValue> cacheManager, string key, string region)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException($"{nameof(key)} empty");
        }
        if (string.IsNullOrWhiteSpace(region))
        {
            throw new ArgumentNullException($"{nameof(region)} empty");
        }
        if (cacheManager.Exists(key, region))
        {
            cacheManager.Remove(key, region);
        }
    }

    public static void Remove<TCacheValue>(this ICacheManager<TCacheValue> cacheManager, string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException($"{nameof(key)} empty");
        }
        if (cacheManager.Exists(key))
        {
            cacheManager.Remove(key);
        }
    }
}