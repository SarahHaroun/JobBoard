using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace JobBoard.Repositories.Redis
{
    public class RedisService : IRedisService
    {
        /*This is the object that connects to Redis.
        * We'll inject it with Dependency Injection from Program.cs.*/

        private readonly IConnectionMultiplexer _redis;

        /*This is the database instance we will use to interact with Redis.
         * Brings the default database to Redis.*/

        private readonly IDatabase _db;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        /*This method sets a value in Redis using a key, with the option to specify an expiry period.
         * It serializes the value to JSON before storing it.*/
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, jsonData, expiry);
        }

        /*This method retrieves a value from Redis using the key.
         * It deserializes the JSON data back to the original type.*/

        public async Task<T?> GetAsync<T>(string key)
        {
            var jsonData = await _db.StringGetAsync(key);
            if (jsonData.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(jsonData!);
        }

        /*This method removes a value from Redis using the key.
         * It returns true if the key was removed, false if it did not exist.*/
        public async Task<bool> RemoveAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }




        public async Task AddToListAsync<T>(string key, T value)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _db.ListRightPushAsync(key, jsonData);
        }

  
        public async Task<List<T>> GetListAsync<T>(string key)
        {
            var items = await _db.ListRangeAsync(key);
            var result = new List<T>();

            foreach (var item in items)
            {
                result.Add(JsonSerializer.Deserialize<T>(item!)!);
            }

            return result;
        }


        public async Task SetExpiryAsync(string key, TimeSpan expiry)
        {
            await _db.KeyExpireAsync(key, expiry);
        }

        public async Task RemoveFromListAsync<T>(string key, T value)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _db.ListRemoveAsync(key, jsonData);
        }
    }
}
