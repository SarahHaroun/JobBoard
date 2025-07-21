using JobBoard.Domain.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
	}
}
