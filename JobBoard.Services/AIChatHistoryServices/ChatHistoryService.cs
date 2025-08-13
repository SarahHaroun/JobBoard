using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JobBoard.Domain.DTO.AIEmbeddingDto;
using StackExchange.Redis;

namespace JobBoard.Services.AIChatHistoryServices
{
    public class ChatHistoryService : IChatHistoryService
    {

        private readonly IDatabase _redis;
        public ChatHistoryService(string redisConnection)
        {
            _redis = ConnectionMultiplexer.Connect(redisConnection).GetDatabase();
        }
   

  public async Task<List<ChatMessageDto>> GetRecentMessagesAsync(string userId, int lastN = 10)
        {
            var entries = await _redis.ListRangeAsync($"chat:{userId}", -lastN, -1);

            // Deserialize each RedisValue to ChatMessageDto
            return entries
                .Select(e => JsonSerializer.Deserialize<ChatMessageDto>(e.ToString()))
                .Where(dto => dto != null) // Filter out any null deserialization results
                .ToList();
        }

        public Task SaveMessageAsync(string userId, string role, string content)
        {
            throw new NotImplementedException();
        }

        public Task ClearHistoryAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
