using Microsoft.Extensions.Caching.Memory;

namespace Mustang.Common.Cache;

public interface ICacheManager
{
    Task<T> Get<T>(string key, Func<T> getter);

    Task Remove(string key);
}