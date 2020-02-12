namespace Common.Caching
{
    using System;

    public static class CacheResolver
    {
        public static ICacheManager GetCacheProvider(bool isInMemory)
        {
            if (isInMemory)
            {
                return new InMemoryCacheManager();
            }

            throw new ArgumentNullException(nameof(isInMemory), "Connection string should be passed for isInMemory as false");
        }
        public static ICacheManager GetCacheProvider(bool isInMemory, string connectionString)
        {
            if (isInMemory)
            {
                return new InMemoryCacheManager();
            }

            if (connectionString != null)
            {
                return new DistributedCacheManager(connectionString);
            }

            throw new ArgumentNullException(nameof(connectionString));
        }
    }
}
