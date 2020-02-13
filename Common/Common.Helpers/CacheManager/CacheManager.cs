namespace Common.Helpers.CacheManager
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Caching;

    /// <summary>
    /// The Cache Manager
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class CacheManager : ICacheManager
    {
        /// <summary>
        /// The Current Cache Manager
        /// </summary>
        public static readonly CacheManager Current = new CacheManager();

        /// <summary>
        /// The Cache
        /// </summary>
        private readonly ObjectCache cache;

        /// <summary>
        /// Prevents a default instance of the <see cref="CacheManager"/> class from being created
        /// </summary>
        private CacheManager()
        {
            this.cache = MemoryCache.Default;
        }

        /// <summary>
        /// Insert into Cache
        /// </summary>
        /// <param name="key">The Cache Key</param>
        /// <param name="data">The Data</param>
        /// <param name="expiration">The Expiration</param>
        public void Insert(string key, object data, TimeSpan expiration)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now.Add(expiration))
            };

            var item = new CacheItem(key, data);
            this.cache.Add(item, policy);
        }

        /// <summary>
        /// Remove from Cache
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The object</returns>
        public object Remove(string key)
        {
            return this.cache.Remove(key);
        }

        /// <summary>
        /// Gets from the Cache
        /// </summary>
        /// <typeparam name="T">The type T</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The object T</returns>
        public T Get<T>(string key)
        {
            try
            {
                return (T)this.cache[key];
            }
            catch
            {
                Trace.TraceError("Error while getting data from cache with key {0}", key);
            }

            return default(T);
        }
    }
}
