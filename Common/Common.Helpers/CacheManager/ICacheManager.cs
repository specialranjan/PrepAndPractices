using System;

namespace Common.Helpers.CacheManager
{
    public interface ICacheManager
    {
        void Insert(string key, object data, TimeSpan expiration);

        object Remove(string key);
    }
}
