using Microsoft.Extensions.Caching.Memory;

namespace Mustang.Common.Cache;

public class LocalCache : ICache
{
    private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

    public Task<T> Get<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        if (_memoryCache.TryGetValue<T>(key, out var value))
            return Task.FromResult(value);

        return Task.FromResult(value);
    }

    public Task Set_NotExpire<T>(string key, T value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        _memoryCache.Remove(key);

        _memoryCache.Set(key, value);

        return Task.CompletedTask;
    }

    public Task Set_SlidingExpire<T>(string key, T value, TimeSpan span)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        _memoryCache.Remove(key);

        _memoryCache.Set(key, value, new MemoryCacheEntryOptions()
        {
            SlidingExpiration = span
        });

        return Task.CompletedTask;
    }

    public Task Set_AbsoluteExpire<T>(string key, T value, TimeSpan span)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        _memoryCache.Remove(key);

        _memoryCache.Set(key, value, span);

        return Task.CompletedTask;
    }

    public Task Set_SlidingAndAbsoluteExpire<T>(string key, T value, TimeSpan slidingSpan, TimeSpan absoluteSpan)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        _memoryCache.Remove(key);

        _memoryCache.Set(key, value, new MemoryCacheEntryOptions()
        {
            SlidingExpiration = slidingSpan,
            AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(absoluteSpan.TotalMilliseconds)
        });

        return Task.CompletedTask;
    }

    public Task Remove(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        _memoryCache.Remove(key);

        return Task.CompletedTask;
    }
}