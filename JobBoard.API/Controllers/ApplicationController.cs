using AutoMapper;
using JobBoard.Domain.DTO.ApplicationDto;
using JobBoard.Domain.Services.Contract;
using JobBoard.Services.SeekerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationController : ControllerBase
	{
		private readonly IApplicationService _applicationService;
		private readonly ISeekerService _seekerService;
		
		private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		public ApplicationController(IApplicationService applicationService, ISeekerService seekerService)
		{
			_applicationService = applicationService;
			_seekerService = seekerService;
		}


		// POST: api/applications
		[HttpPost]
		[Authorize(Roles = "Seeker")]
		public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDto createDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (createDto.JobId <= 0)
				return BadRequest("Invalid Job Id.");

			if (userId == null)
				return Unauthorized();

			var seekerProfile = await _seekerService.GetByUserIdAsync(userId);
			if (seekerProfile == null)
				return Unauthorized("Seeker profile not found");

			var createdApplication = await _applicationService.CreateApplicationAsync(createDto, seekerProfile.Id);
			if (createdApplication == null)
				return BadRequest("Unable to create application. You may have already applied to this job or the job doesn't exist.");

			return CreatedAtAction(nameof(GetMyApplications), new { id = createdApplication.Id }, createdApplication);
		}

		// GET: api/applications/my-applications
		[HttpGet("my-applications")]
		[Authorize(Roles = "Seeker")]
		public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetMyApplications()
		{
			if (userId == null)
				return Unauthorized();

			var seekerProfile = await _seekerService.GetByUserIdAsync(userId);
			if (seekerProfile == null)
				return Unauthorized("Seeker profile not found");

			var applications = await _applicationService.GetApplicationsByApplicantIdAsync(seekerProfile.Id);
			return Ok(applications);
		}

		// GET: api/applications/has-applied/{jobId}
		[HttpGet("has-applied/{jobId}")]
		[Authorize(Roles = "Seeker")]
		public async Task<ActionResult<bool>> HasApplied(int jobId)
		{
			if (jobId <= 0)
				return BadRequest("Invalid JobId.");

			if (userId == null)
				return Unauthorized();

			var seekerProfile = await _seekerService.GetByUserIdAsync(userId);
			if (seekerProfile == null)
				return Unauthorized("Seeker profile not found");

			var hasApplied = await _applicationService.HasUserAppliedToJobAsync(seekerProfile.Id, jobId);
			return Ok(hasApplied);
		}
	}
}