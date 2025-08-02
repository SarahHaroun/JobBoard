using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.Services.Contract;
using JobBoard.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobsController : ControllerBase
	{
		private readonly IJobService _jobService;

		public JobsController(IJobService jobService)
		{
			_jobService = jobService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllJobs()
		{
			var result = await _jobService.GetAllJobsAsync();
			return Ok(result);

		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetJob(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid job ID.");

			var result = await _jobService.GetJobByIdAsync(id);
			
			if (result == null)
				return NotFound();

			return Ok(result);
		}

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetJobsByCategoryId(int categoryId)
        {
            var jobs = await _jobService.GetJobsByCategoryIdAsync(categoryId);
            return Ok(jobs);
        }

        [HttpPost]
        public async Task<IActionResult> AddJob([FromBody] JobDto jobDto)
        {
            if (jobDto == null)
                return BadRequest("Job data is required.");

            var createdJob = await _jobService.AddJobAsync(jobDto);
            return CreatedAtAction(nameof(GetJob), new { id = createdJob.Id }, createdJob);
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> GetJobDetailsById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid job ID.");

            var job = await _jobService.GetJobDetailsByIdAsync(id);
            if (job == null)
                return NotFound("Job not found.");

            return Ok(job);
        }
    }
}
