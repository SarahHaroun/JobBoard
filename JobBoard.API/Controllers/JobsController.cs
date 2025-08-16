using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.Services.Contract;
using JobBoard.Domain.Shared;
using JobBoard.Services;
using JobBoard.Services.EmployerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Employer")]
	public class JobsController : ControllerBase
	{
		private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		private readonly IJobService _jobService;
		private readonly IEmployerService _employerService;

		public JobsController(IJobService jobService, IEmployerService employerService)
		{
			_jobService = jobService;
			_employerService = employerService;
		}


		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllJobs([FromQuery] JobFilterParams filterParams)
		{
			var result = await _jobService.GetAllJobsAsync(filterParams);
			return Ok(result);
		}

		[HttpGet("{id:int}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetJob(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid job ID.");

			var result = await _jobService.GetJobByIdAsync(id);

			if (result == null)
				return NotFound();

			return Ok(result);
		}

		//POST: api/jobs
		[HttpPost]
		public async Task<IActionResult> AddJob([FromBody] CreateUpdateJobDto jobDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (userId == null)
				return Unauthorized();


			var employer = await _employerService.GetByUserId(userId);
			if (employer == null)
				return Unauthorized("Employer profile not found");

			var createdJOb = await _jobService.AddJobAsync(jobDto, employer.Id);

			return CreatedAtAction(nameof(GetJob), new { id = createdJOb.Id }, createdJOb);
		}

		//PUT: api/jobs/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> EditJob(int id, [FromBody] CreateUpdateJobDto jobDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (userId == null)
				return Unauthorized();

			var employer = await _employerService.GetByUserId(userId);
			if (employer == null)
				return Unauthorized("You are not authorized to edit this job.");
			
			var updatedJob = await _jobService.UpdateJobAsync(id, jobDto, employer.Id);

			if (updatedJob == null)
				return NotFound("Job not found or you don't have permission to edit it.");

			return Ok(updatedJob);
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteJob([FromRoute] int id)
		{
			if (userId == null)
				return Unauthorized();

			var employer = await _employerService.GetByUserId(userId);
			if (employer == null)
				return BadRequest("Employer profile not found.");
			var deletedJob = await _jobService.DeleteJobAsync(id);
			if (!deletedJob)
				return NotFound();

			return NoContent();

		}

		[HttpGet("categories")]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllCategories()
		{
			var result = await _jobService.GetAllCategoriesAsync();
			return Ok(result);
		}

		[HttpGet("skills")]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllSkills()
		{
			var result = await _jobService.GetAllSkillsAsync();
			return Ok(result);
		}

	}
}
