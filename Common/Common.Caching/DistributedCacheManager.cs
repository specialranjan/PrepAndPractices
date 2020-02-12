namespace Common.Caching
{
    using System;
    using System.Runtime.Caching;
    using System.Threading.Tasks;

    using CachingFramework.Redis;
    using StackExchange.Redis;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using System.Linq;

    public class DistributedCacheManager : ICacheManager, IDisposable
    {
        private readonly Lazy<Context> cacheContext;
        public DistributedCacheManager(string connectionString)
        {
            this.cacheContext = new Lazy<Context>(() => new Context(connectionString));
        }
        public void AddOrUpdate<T>(string key, T value, TimeSpan slidingExpirationTime)
        {
            ValidateKey(key);
            this.cacheContext.Value.Cache.SetObject(key, value, slidingExpirationTime);
        }
        public Task AddOrUpdateAsync<T>(string key, T value, TimeSpan slidingExpirationTime)
        {
            ValidateKey(key);
            return this.cacheContext.Value.Cache.SetObjectAsync(key, value, slidingExpirationTime);
        }
        public void AddOrUpdate<T>(string key, T value, CacheItemPolicy policy)
        {
            ValidateKey(key);
            if (policy != null)
            {
                this.AddOrUpdate(key, value, TimeSpan.Compare(policy.SlidingExpiration, TimeSpan.FromSeconds(0)) == 0 ? policy.AbsoluteExpiration - DateTimeOffset.UtcNow : policy.SlidingExpiration);
            }
            else
            {
                this.cacheContext.Value.Cache.SetObject(key, value);
            }
        }
        public async Task AddOrUpdateAsync<T>(string key, T value, CacheItemPolicy policy)
        {
            ValidateKey(key);

            if (policy != null)
            {
                await this.AddOrUpdateAsync(key, value, TimeSpan.Compare(policy.SlidingExpiration, TimeSpan.FromSeconds(0)) == 0 ? policy.AbsoluteExpiration - DateTimeOffset.UtcNow : policy.SlidingExpiration).ConfigureAwait(false);
            }
            else
            {
                await this.cacheContext.Value.Cache.SetObjectAsync(key, value).ConfigureAwait(false);
            }
        }
        public void Delete(string key)
        {
            ValidateKey(key);
            if (this.IsExistAsync(key).Result)
            {
                this.cacheContext.Value.Cache.Remove(key);
            }
        }
        public async Task DeleteAsync(string key)
        {
            ValidateKey(key);
            if (await this.IsExistAsync(key).ConfigureAwait(false))
            {
                await this.cacheContext.Value.Cache.RemoveAsync(key).ConfigureAwait(false);
            }
        }
        public T Get<T>(string key)
        {
            ValidateKey(key);
            return this.cacheContext.Value.Cache.GetObject<T>(key);
        }
        public Task<T> GetAsync<T>(string key)
        {
            ValidateKey(key);
            return this.cacheContext.Value.Cache.GetObjectAsync<T>(key);
        }
        public async Task<IDictionary<string, T>> GetMultipleAsync<T>(string[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            ValidateKeys(keys);
            var keyList = new List<RedisKey>();
            foreach (var item in keys)
            {
                keyList.Add(item);
            }
            var batchingcachecontext = cacheContext.Value.GetConnectionMultiplexer();
            var values = await batchingcachecontext.GetDatabase().StringGetAsync(keyList.ToArray()).ConfigureAwait(false);
            IDictionary<string, T> result = new Dictionary<string, T>();
            for (int i = 0; i < keys.Length; i++)
            {
                result.Add(keys[i], values[i].ToString() == null ? default(T) : JsonConvert.DeserializeObject<T>(values[i].ToString()));
            }

            return result;
        }
        public async Task AddMultipleAsync<T>(IList<Tuple<string, T>> keyValue, TimeSpan cacheTimeSpan)
        {
            if (keyValue == null)
            {
                throw new ArgumentNullException(nameof(keyValue));
            }

            ValidateKeys(keyValue.Select(item => item.Item1).ToArray());
            var keyValuePairs = new List<KeyValuePair<RedisKey, RedisValue>>();
            foreach (var item in keyValue)
            {
                keyValuePairs.Add(new KeyValuePair<RedisKey, RedisValue>(item.Item1, JsonConvert.SerializeObject(item.Item2)));
            }
            var batchingcachecontext = cacheContext.Value.GetConnectionMultiplexer();
            await batchingcachecontext.GetDatabase().StringSetAsync(keyValuePairs.ToArray()).ConfigureAwait(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.cacheContext.Value?.Dispose();
            }
        }
        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentOutOfRangeException(nameof(key), "Cache key is required");
            }
        }
        private static void ValidateKeys(string[] keys)
        {
            if (keys.Length <= 0 || keys.All(str => string.IsNullOrEmpty(str)))
            {
                throw new ArgumentOutOfRangeException(nameof(keys), "Cache key is required");
            }
        }
        private Task<bool> IsExistAsync(string key)
        {
            return this.cacheContext.Value.Cache.KeyExistsAsync(key);
        }
    }
}
