using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.DTO.AIEmbeddingDto;
using JobBoard.Domain.Entities;

namespace JobBoard.Services.AIEmbeddingService
{
    public interface IAIEmbeddingService
    {
        public Task GenerateEmbeddingsForJobsAsync();
        public Task<List<SemanticSearchResultDto>> SearchJobsByMeaningAsync(string query, int topK);
        public Task<string> GetJobAnswerFromGeminiAsync(string userQuestion);
        public Task<string> GetJobAnswerFromGeminiAsync(string userId, string userQuestion);

        public Task GenerateEmbeddingForJobAsync(Job job);

    }
}
