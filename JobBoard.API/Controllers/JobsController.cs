using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.Services.Contract;
using JobBoard.Domain.Shared;
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
		public async Task<IActionResult> GetAllJobs([FromQuery] JobFilterParams filterParams)
		{
			var result = await _jobService.GetAllJobsAsync(filterParams);
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

        [HttpPost]
        public async Task<IActionResult> AddJob([FromBody] JobDto jobDto)
        {
            if (jobDto == null)
                return BadRequest("Job data is required.");

            var createdJob = await _jobService.AddJobAsync(jobDto);
            return CreatedAtAction(nameof(GetJob), new { id = createdJob.Id }, createdJob);
        }
    }
}
