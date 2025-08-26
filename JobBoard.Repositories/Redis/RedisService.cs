using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly JsonSerializerOptions _options;
        private readonly ILogger<RedisService> _logger;

        public RedisService(IConnectionMultiplexer redis, ILogger<RedisService> logger)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
            // Define options once here
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true // Optional for readability
            };
        }

        /*This method sets a value in Redis using a key, with the option to specify an expiry period.
         * It serializes the value to JSON before storing it.*/
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (value == null) return;
            var expiryTime = expiry ?? TimeSpan.FromDays(1); // Default to 1 day if no expiry is provided
            // Create a JsonSerializerOptions object to configure serialization settings

          
            // Serialize the value to JSON
            var jsonData = JsonSerializer.Serialize(value,_options);
            // Store the JSON data in Redis with the specified expiry time
            await _db.StringSetAsync(key, jsonData, expiry);
        }

        /*This method retrieves a value from Redis using the key.
         * It deserializes the JSON data back to the original type.*/

        public async Task<T?> GetAsync<T>(string key)
        {
            var jsonData = await _db.StringGetAsync(key);
            if (jsonData.IsNullOrEmpty)
                return default;

            if (typeof(T) == typeof(string))
            {
                return (T)(object)jsonData.ToString();
            }
          
            // Deserialize the JSON data back to the original type
            var value = JsonSerializer.Deserialize<T>(jsonData, _options);
            return value;
        }

        /*This method removes a value from Redis using the key.
         * It returns true if the key was removed, false if it did not exist.*/
        public async Task<bool> RemoveAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }
        /*--------------------------------------------------------------------------------*/
        public async Task DeleteByPrefixAsync(string prefix)
        {
            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints().First()); // Get Redis Server for Keys scanning
                var keys = server.Keys(pattern: $"{prefix}*"); // Find all keys starting with prefix
                foreach (var key in keys)
                {
                    await _db.KeyDeleteAsync(key);
                }
                _logger.LogInformation($"Deleted {keys.Count()} keys with prefix: {prefix}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting by prefix {prefix}: {ex.Message}");
            }
        }

        /* ----------------------------------- List operations --------------------------------- */

        public async Task AddToListAsync<T>(string key, T value)
        {

            try
            {
                var jsonData = JsonSerializer.Serialize(value, _options);
                await _db.ListRightPushAsync(key, jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding to list {key}: {ex.Message}");
                throw; // Re-throw to handle in caller
            }
        }


        public async Task<List<T>> GetListAsync<T>(string key)
        {
            try
            {
                var items = await _db.ListRangeAsync(key);
                var result = new List<T>();

                foreach (var item in items)
                {
                    if (item.IsNullOrEmpty) continue; // Skip empty items
                    var deserialized = JsonSerializer.Deserialize<T>(item!, _options); 
                    if (deserialized != null)
                        result.Add(deserialized);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting list {key}: {ex.Message}");
                return new List<T>(); // Return empty list on error
            }
        }


        public async Task SetExpiryAsync(string key, TimeSpan expiry)
        {
            await _db.KeyExpireAsync(key, expiry);
        }

        public async Task RemoveFromListAsync<T>(string key, T value , long count = 0)
        {
            if (value == null) return;
            try
            {
                var jsonData = JsonSerializer.Serialize(value, _options); 
                var removedCount = await _db.ListRemoveAsync(key, jsonData , count);
                if (removedCount == 0)
                {
                    _logger.LogWarning($"No items removed from list {key} for value: {jsonData}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing from list {key}: {ex.Message}");
                throw;
            }
        }
    }
}
