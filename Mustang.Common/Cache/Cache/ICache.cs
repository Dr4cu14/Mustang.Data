namespace Mustang.Common.Cache;

public interface ICache
{
    Task<T> Get<T>(string key);

    Task Set_NotExpire<T>(string key, T value);

    Task Set_SlidingExpire<T>(string key, T value, TimeSpan span);


    Task Set_AbsoluteExpire<T>(string key, T value, TimeSpan span);


    Task Set_SlidingAndAbsoluteExpire<T>(string key, T value, TimeSpan slidingSpan, TimeSpan absoluteSpan);


    Task Remove(string key);
}