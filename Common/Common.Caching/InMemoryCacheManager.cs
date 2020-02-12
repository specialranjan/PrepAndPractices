namespace Common.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Threading.Tasks;

    public class InMemoryCacheManager : ICacheManager, IDisposable
    {
        private const string DefaultCacheName = "Default";
        private MemoryCache memoryCache;
        public InMemoryCacheManager()
        {
            this.memoryCache = MemoryCache.Default;
        }
        public InMemoryCacheManager(string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName))
            {
                throw new ArgumentNullException(nameof(cacheName));
            }

            this.memoryCache = new MemoryCache(cacheName);
        }
        public void AddOrUpdate<T>(string key, T value, TimeSpan slidingExpirationTime)
        {
            this.AddOrUpdate(key, value, new CacheItemPolicy { SlidingExpiration = slidingExpirationTime });
        }
        public Task AddOrUpdateAsync<T>(string key, T value, TimeSpan slidingExpirationTime)
        {
            return Task.Run(() => this.AddOrUpdate(key, value, new CacheItemPolicy { SlidingExpiration = slidingExpirationTime }));
        }
        public void AddOrUpdate<T>(string key, T value, CacheItemPolicy policy)
        {
            if (object.Equals(value, default(T)))
            {
                throw new ArgumentNullException(nameof(value));
            }

            ValidateKey(key);
            this.memoryCache.Set(key, value, policy);
        }
        public Task AddOrUpdateAsync<T>(string key, T value, CacheItemPolicy policy)
        {
            return Task.Run(() => this.AddOrUpdate(key, value, policy));
        }
        public void Delete(string key)
        {
            ValidateKey(key);
            this.memoryCache.Remove(key);
        }
        public Task DeleteAsync(string key)
        {
            ValidateKey(key);
            return Task.Run(() => this.memoryCache.Remove(key));
        }
        public T Get<T>(string key)
        {
            ValidateKey(key);
            var item = this.memoryCache.GetCacheItem(key);
            if (item == null)
            {
                return default(T);
            }

            return (T)item.Value;
        }
        public Task<T> GetAsync<T>(string key)
        {
            ValidateKey(key);
            var item = this.memoryCache.GetCacheItem(key);
            return item == null ? Task.FromResult(default(T)) : Task.FromResult((T)item.Value);
        }
        public void Clear()
        {
            if (!this.memoryCache.Name.Equals(DefaultCacheName, StringComparison.OrdinalIgnoreCase))
            {
                var previousCache = this.memoryCache;
                var cacheName = this.memoryCache.Name;
                this.memoryCache = new MemoryCache(cacheName);
                previousCache.Dispose();
            }
            else
            {
                throw new InvalidOperationException("Default Memory Cache cannot be cleared using the current method.");
            }
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
                this.memoryCache?.Dispose();
            }
        }
        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentOutOfRangeException(nameof(key), "Cache key is required");
            }
        }
        public async Task<IDictionary<string, T>> GetMultipleAsync<T>(string[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            ValidateKeys(keys);
            IDictionary<string, T> item = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                var value = this.memoryCache.GetCacheItem(key);
                item.Add(key, value == null ? default(T) : (T)value.Value);
            }

            return item;
        }
        public async Task AddMultipleAsync<T>(IList<Tuple<string, T>> keyValue, TimeSpan cacheTimeSpan)
        {
            if (keyValue == null)
            {
                throw new ArgumentNullException(nameof(keyValue));
            }

            foreach (var vindetails in keyValue)
            {
                await this.AddOrUpdateAsync<T>(vindetails.Item1, vindetails.Item2, cacheTimeSpan).ConfigureAwait(false);
            }
        }

        private static void ValidateKeys(string[] keys)
        {
            if (keys.Length <= 0 || keys.All(str => string.IsNullOrEmpty(str)))
            {
                throw new ArgumentOutOfRangeException(nameof(keys), "Cache key is required");
            }
        }
    }
}
