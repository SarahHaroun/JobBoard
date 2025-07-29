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
    }
}
