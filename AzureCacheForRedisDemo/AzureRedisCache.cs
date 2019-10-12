using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace AzureCacheForRedisDemo
{
    public class AzureRedisCache
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = 
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("insert connection string here"));

        public static ConnectionMultiplexer Connection => lazyConnection.Value;

        private static readonly string keyAppendage = "_RedisCacheKey_Dev";
        private static readonly JsonSerializerSettings jsonSerializerSettings =
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

        // Get entity of Type T from the Redis Cache
        public static T GetEntity<T>(string key, Func<string, T> getFromDataStore, TimeSpan? expirationTimeSpan = null) where T : class
        {
            IDatabase cache = Connection.GetDatabase();
            string entityCacheKey = key + keyAppendage;

            RedisValue valueInCache = cache.StringGet(entityCacheKey);
            T entity = string.IsNullOrWhiteSpace(valueInCache) ? null : JsonConvert.DeserializeObject<T>(valueInCache);

            if(entity == null) // Not in Cache
            {
                // In the case of a cache miss, this retrieves the entity from the original data store
                entity = getFromDataStore(key);

                if(entity != null)
                {
                    cache.StringSet(entityCacheKey, JsonConvert.SerializeObject(entity, Formatting.Indented, jsonSerializerSettings));
                    if(expirationTimeSpan != null)
                        cache.KeyExpire(entityCacheKey, expirationTimeSpan);
                }
            }

            return entity;
        }
    
        // Delete entity from the Redis Cache
        public static void DeleteEntity(string key)
        {
            IDatabase cache = Connection.GetDatabase();
            string entityCacheKey = key + keyAppendage;

            // Delete Key from Redis Cache
            cache.KeyDelete(entityCacheKey);
        }

        // Update entity of Type T in the Redis Cache
        public static void UpdateEntity<T>(string key, T entity, Action<T> updateEntityInDataStore, TimeSpan? expirationTimeSpan = null) where T : class
        {
            updateEntityInDataStore(entity);

            IDatabase cache = Connection.GetDatabase();
            string entityCacheKey = key + keyAppendage;

            // Delete Key from Redis Cache
            cache.KeyDelete(entityCacheKey);

            // Set the new value to the key
            cache.StringSet(entityCacheKey, JsonConvert.SerializeObject(entity, Formatting.Indented, jsonSerializerSettings), expirationTimeSpan);
        }
    }
}



        

