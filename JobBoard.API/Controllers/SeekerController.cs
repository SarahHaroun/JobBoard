using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Services.SeekerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Seeker")]
	public class SeekerController : ControllerBase
	{
		private readonly ISeekerService seekerService;
		private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		public SeekerController(ISeekerService seekerService)
		{
			this.seekerService = seekerService;
		}

		/*------------------------Get All Profiles --------------------------*/
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var seekers = await seekerService.GetAll();
			return Ok(seekers);
		}

		/*------------------------Get my Profile --------------------------*/
		[HttpGet("GetMyProfile")]
		public async Task<IActionResult> GetMyProfile()
		{
			// Check if the userId is null, which means the user is not authenticated
			if (userId == null)
				return Unauthorized();
			var seekerProfile = await seekerService.GetByUserId(userId);
			// If the profile is not found, return NotFound
			if (seekerProfile == null)
				return NotFound();
			// If the profile is found, return it
			return Ok(seekerProfile);
		}



		/*------------------------create --------------------------*/
		//[HttpPost]
		//public async Task<IActionResult> Create([FromBody] SeekerProfileDto dto)
		//{
		//	// Check if the userId is null, which means the user is not authenticated
		//	if (userId == null)
		//		return Unauthorized();

		//	// Check if a seeker profile already exists for the authenticated user
		//	var existingProfile = await seekerService.GetByUserId(userId);
		//	if (existingProfile != null)
		//		return BadRequest("Seeker profile already exists");
		//	// If no profile exists, create a new one
		//	dto.UserId = userId; // Set the UserId from the authenticated user
		//	var result = await seekerService.Create(dto);
		//	if (!result.Success)
		//		return BadRequest(result.Message);
		//	return Ok(result.Data);
		//	//CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
		//}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] SeekerProfileUpdateDto dto)
		{
			// Check if the userId is null, which means the user is not authenticated
			if (userId == null)
				return Unauthorized();
			// Check if a seeker profile exists for the authenticated user
			var existingProfile = await seekerService.GetByUserId(userId);
			if (existingProfile == null)
				return NotFound();
			// If a profile exists, update it
			//dto.UserId = userId; // Ensure the UserId is set
			var result = await seekerService.Update(existingProfile.Id, dto);
			if (!result)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteById(int id)
		{
			var result = await seekerService.DeleteById(id);
			if (!result)
				return NotFound();
			return Ok("Deleted");
		}
	}
}
