using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Common.Caching
{
    public interface ICacheManager
    {
        void AddOrUpdate<T>(string key, T value, TimeSpan slidingExpirationTime);
        Task AddOrUpdateAsync<T>(string key, T value, TimeSpan slidingExpirationTime);
        void AddOrUpdate<T>(string key, T value, CacheItemPolicy policy);
        Task AddOrUpdateAsync<T>(string key, T value, CacheItemPolicy policy);
        void Delete(string key);
        Task DeleteAsync(string key);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        Task<IDictionary<string, T>> GetMultipleAsync<T>(string[] keys);
        Task AddMultipleAsync<T>(IList<Tuple<string, T>> keyValue, TimeSpan cacheTimeSpan);
    }
}
