using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Redis
{
    public interface IRedisService
    {
        // String key-value

        /* 
         * Stores a value in Redis using a key, with the option to specify an expiry period.
         * If expiry is not specified, the value will not expire.
         */
       
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

        //Returns the value from Redis using the key.
        Task<T?> GetAsync<T>(string key);

        //removes the value from Redis using the key.
        Task<bool> RemoveAsync(string key);

        public Task DeleteByPrefixAsync(string prefix);
        // List operations
        Task AddToListAsync<T>(string key, T value); // Push to list
        Task<List<T>> GetListAsync<T>(string key);   // Get all items from list
        Task SetExpiryAsync(string key, TimeSpan expiry); // Set expiration for list
    }
}
