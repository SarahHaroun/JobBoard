using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JobBoard.Domain.DTO.AIEmbeddingDto;
using JobBoard.Repositories.Redis;
using StackExchange.Redis;

namespace JobBoard.Services.AIChatHistoryServices
{
    public class ChatHistoryService : IChatHistoryService
    {
        private readonly IRedisService _redisService;

        // Define the time to live for chat history , the chat will expire after this time
        private readonly TimeSpan _chatExpiry = TimeSpan.FromHours(1);

        public ChatHistoryService(IRedisService redisService)
        {
            _redisService = redisService;
        }


        private string GetRedisKey(string userId)
        {
            return $"chat:{userId}";
        }

        public async Task AddMessageAsync(string userId, string message)
        {
            var key = GetRedisKey(userId);

            // Retrieve old messages 
            var messages = await _redisService.GetAsync<List<string>>(key) ?? new List<string>();
            // Add the new message to the list
            messages.Add(message);
            // Save the updated list back to Redis with an expiry time
            await _redisService.SetAsync(key, messages, _chatExpiry);

        }



        public Task<List<string>> GetMessagesAsync(string userId)
        {
           var key = GetRedisKey(userId);
            // Retrieve the messages from Redis
            return _redisService.GetAsync<List<string>>(key) ?? Task.FromResult(new List<string>());
        }

        public Task ClearHistoryAsync(string userId)
        {
            var key = GetRedisKey(userId);
            // Remove the chat history from Redis
            return _redisService.RemoveAsync(key);
        }
    }
}
