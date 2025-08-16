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
        private readonly TimeSpan _chatExpiry = TimeSpan.FromHours(5);

        // Define the maximum number of messages to keep in history
        private const int MaxToKeep = 40;


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

        /*-------------------------------------------------------------------------------*/
        /*------------------------------------Chat History-------------------------------------------*/
        public async Task AddAsync(string userId, ChatMessageDto message)
        {
            var key = GetRedisKey(userId);
            // Retrieve old messages 
            var messages = await _redisService.GetAsync<List<ChatMessageDto>>(key) ?? new List<ChatMessageDto>();
            // Add the new message to the list
            messages.Add(message);
            // If the number of messages exceeds MaxToKeep, remove the oldest ones
            if (messages.Count > MaxToKeep)
            {
                messages = messages.Skip(Math.Max(0, messages.Count - MaxToKeep)).ToList();
            }
            // Save the updated list back to Redis with an expiry time
            await _redisService.SetAsync(key, messages, _chatExpiry);
        }
        public async Task<IReadOnlyList<ChatMessageDto>> GetAsync(string userId, int takeLast = 20)
        {
            var key = GetRedisKey(userId);
            // Retrieve the messages from Redis
            var messages = await _redisService.GetAsync<List<ChatMessageDto>>(key) ?? new List<ChatMessageDto>();
            // If takeLast is greater than the number of messages, adjust it
            if (takeLast > messages.Count)
            {
                takeLast = messages.Count;
            }
            // Return the last 'takeLast' messages
            return messages.TakeLast(takeLast).ToList();
        }
        public Task ClearAsync(string userId)
        {
            var key = GetRedisKey(userId);
            // Remove the chat history from Redis
            return _redisService.RemoveAsync(key);
        }
    }
}
