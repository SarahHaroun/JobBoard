using AutoMapper;
using JobBoard.Domain.DTO.ApplicationDto;
using JobBoard.Domain.Services.Contract;
using JobBoard.Services.SeekerService;
using JobBoard.Services.EmployerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobBoard.Domain.Shared;
using JobBoard.Domain.Entities.Enums;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationController : ControllerBase
	{
		private readonly IApplicationService _applicationService;
		private readonly ISeekerService _seekerService;
		private readonly IEmployerService _employerService;

		private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		public ApplicationController(IApplicationService applicationService, ISeekerService seekerService, IEmployerService employerService)
		{
			_applicationService = applicationService;
			_seekerService = seekerService;
			_employerService = employerService;
		}

		//--------------------------All----------------------
		// GET: api/application/{id}
		[HttpGet("{id}")]
		[Authorize]
		public async Task<ActionResult<ApplicationDto>> GetApplicationById(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid application Id.");

			var application = await _applicationService.GetApplicationByIdAsync(id);
			if (application == null)
				return NotFound("Application not found.");

			// Check authorization - only the applicant, job owner, or admin can view
			if (User.IsInRole("Admin"))
				return Ok(application);

			if (User.IsInRole("Seeker"))
			{
				var seekerProfile = await _seekerService.GetByUserIdAsync(userId);
				if (seekerProfile != null && application.ApplicantId == seekerProfile.Id)
					return Ok(application);
			}

			if (User.IsInRole("Employer"))
			{
				var employerProfile = await _employerService.GetByUserId(userId);
				if (employerProfile != null && application.Job?.EmployerId == employerProfile.Id)
					return Ok(application);
			}

			return Forbid("You don't have permission to view this application.");
		}

		//--------------------------Seeker----------------------
		// POST: api/application
		[HttpPost]
		[Consumes("multipart/form-data")]
		[Authorize(Roles = "Seeker")]
		public async Task<IActionResult> CreateApplication([FromForm] CreateApplicationDto createDto)
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

			return CreatedAtAction(nameof(GetApplicationById), new { id = createdApplication.Id }, createdApplication);
		}

		// GET: api/application/my-applications
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

		// GET: api/application/has-applied/{jobId}
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


		//--------------------------Admin----------------------
		// DELETE: api/application/{id}
		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteApplication(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid application Id.");

			var deleted = await _applicationService.DeleteApplicationAsync(id);
			if (!deleted)
				return NotFound("Application not found.");

			return NoContent();
		}

	}
}