using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.DTO.AIEmbeddingDto;

namespace JobBoard.Services.AIEmbeddingService
{
    public interface IAIEmbeddingService
    {
        public Task GenerateEmbeddingsForJobsAsync();
        public Task<List<SemanticSearchResultDto>> SearchJobsByMeaningAsync(string query, int topK);

    }
}
