using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.DTO.AIEmbeddingDto;

namespace JobBoard.Services.AIChatHistoryServices
{
    public interface IChatHistoryService
    {
        /*
        /// <summary>
        /// Saves the chat history for a specific job.
        /// </summary>
        /// <param name="jobId">The ID of the job.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="messages">The list of messages in the chat history.</param>
        Task SaveChatHistoryAsync(int jobId, int userId, List<string> messages);
        /// <summary>
        /// Retrieves the chat history for a specific job.
        /// </summary>
        /// <param name="jobId">The ID of the job.</param>
        /// <returns>A list of messages in the chat history.</returns>
        Task<List<string>> GetChatHistoryAsync(int jobId);  */

        Task AddMessageAsync(string userId, string message);
        Task<List<string>> GetMessagesAsync(string userId);
        Task ClearHistoryAsync(string userId);






        /*---------------------Add Chat History---------------------*/
        Task AddAsync(string userId, ChatMessageDto message);
        Task<IReadOnlyList<ChatMessageDto>> GetAsync(string userId, int takeLast = 20);
        Task ClearAsync(string userId);
    }

}
