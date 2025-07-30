using JobBoard.Domain.DTO.AIEmbeddingDto;
using JobBoard.Services.AIEmbeddingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIEmbeddingsController : ControllerBase
    {
        private readonly IAIEmbeddingService _aiEmbeddingService;
        public AIEmbeddingsController(IAIEmbeddingService aiEmbeddingService)
        {
            _aiEmbeddingService = aiEmbeddingService;
        }


        /*Generate Embeddings for all Jobs that don’t have embeddings yet.*/
        [HttpPost("generate-jobs")]
        public async Task<IActionResult> GenerateJobEmbeddings()
        {
            await _aiEmbeddingService.GenerateEmbeddingsForJobsAsync();
            return Ok("Job embeddings generated successfully.");
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchJobsByMeaning([FromQuery] string query, [FromQuery] int topK = 5)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query cannot be empty.");

            var results = await _aiEmbeddingService.SearchJobsByMeaningAsync(query, topK);

            return Ok(results);
        }


        [HttpPost("ask")]
        public async Task<IActionResult> AskGemini([FromBody] AskQuestionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Question))
                return BadRequest("Question is required.");

            var answer = await _aiEmbeddingService.GetJobAnswerFromGeminiAsync(dto.Question);

            return Ok(new { answer });
        }


    }
}
