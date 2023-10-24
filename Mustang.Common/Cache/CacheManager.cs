

namespace Mustang.Common.Cache;

public class CacheManager : ICacheManager
{
    public static readonly ICacheManager Instance = new CacheManager();

    public async Task<T> Get<T>(string key, Func<T> getter)
    {
        var cache = CacheFactory.GetCache();

        T result;
        try
        {
            result = await cache.Get<T>(key);
            if (result == null)
            {
                result = getter();

                if (result != null)
                    await cache.Set_NotExpire(key, result);
            }
        }
        catch (Exception ex)
        {
            await Logger.Logger.Instance.WriteLog($"cacheKey:[{key}],获取数据失败", ex);

            result = getter();
        }

        return result;
    }


    public async Task Remove(string key)
    {
        var cache = CacheFactory.GetCache();

        await cache.Remove(key);
    }
}