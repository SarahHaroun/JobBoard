using JobBoard.Domain.DTO.SavedJobsDto;
using JobBoard.Domain.Services.Contract;
using JobBoard.Domain.Shared;
using JobBoard.Services.SeekerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Seeker")]
	public class SavedJobController : ControllerBase
	{
		private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		private readonly ISavedJobService _savedJobService;
		private readonly ISeekerService _seekerService;

		public SavedJobController(ISavedJobService savedJobService, ISeekerService seekerService)
		{
			_savedJobService = savedJobService;
			_seekerService = seekerService;
		}


		// GET: api/SavedJob
		[HttpGet]
		public async Task<IActionResult> GetAllSavedJobs([FromQuery] SavedJobFilterParams filterParams)
		{
			var seekerId = await GetSeekerIdAsync();
			if (seekerId.Result != null)
				return seekerId.Result;

			var result = await _savedJobService.GetSavedJobsAsync(filterParams);
			return Ok(result);
		}

		// GET: api/SavedJob/{jobId}
		[HttpGet("{jobId:int}")]
		public async Task<IActionResult> GetSavedJob(int jobId)
		{
			var seekerId = await GetSeekerIdAsync();
			if (seekerId.Result != null)
				return seekerId.Result;

			var savedJob = await _savedJobService.GetSavedJobByIdAsync(jobId);
			if (savedJob == null)
				return NotFound();

			return Ok(savedJob);
		}

		// POST: api/SavedJob
		[HttpPost]
		public async Task<IActionResult> SaveJob([FromBody] CreateSavedJobDto savedJobDto)
		{
			var seekerId = await GetSeekerIdAsync();
			if (seekerId.Result != null)
				return seekerId.Result;

			var savedJob = await _savedJobService.SaveJobAsync(seekerId.Value, savedJobDto);
			if (savedJob == null)
				return BadRequest("Job is already saved or does not exist");

			return Ok(savedJob);
		}

		// GET: api/SavedJob/issaved/{jobId}
		[HttpGet("issaved/{jobId:int}")]
		public async Task<IActionResult> IsJobSaved(int jobId)
		{
			var seekerId = await GetSeekerIdAsync();
			if (seekerId.Result != null)
				return seekerId.Result;

			var isSaved = await _savedJobService.IsJobSavedAsync(seekerId.Value, jobId);
			return Ok(isSaved);
		}

		// DELETE: api/SavedJob/{jobId}
		[HttpDelete("{jobId:int}")]
		public async Task<IActionResult> UnsaveJob(int jobId)
		{
			var seekerId = await GetSeekerIdAsync();
			if (seekerId.Result != null)
				return seekerId.Result;

			var deleted = await _savedJobService.UnsaveJobAsync(seekerId.Value, jobId);
			if (!deleted)
				return NotFound();

			return NoContent();
		}

		// method to get seekerId or return Unauthorized
		private async Task<ActionResult<int>> GetSeekerIdAsync()
		{
			if (userId == null)
				return Unauthorized();

			var seeker = await _seekerService.GetByUserIdAsync(userId);
			if (seeker == null)
				return Unauthorized("Seeker profile not found");

			return seeker.Id;
		}
	}
}
