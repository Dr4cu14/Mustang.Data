using System.Collections.Concurrent;

namespace Mustang.Common.Cache;

public class CacheFactory
{
    private static readonly ConcurrentDictionary<string, ICache> LogEmitters = new();

    static CacheFactory()
    {
        LogEmitters.TryAdd(nameof(LocalCache).ToString(), new LocalCache());
    }

    public static ICache GetCache()
    {
        if (LogEmitters.TryGetValue(nameof(LocalCache), out var cacheManager))
            return cacheManager;

        ;
        throw new Exception("ICacheManager not found");
    }
}