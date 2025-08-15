using JobBoard.Domain.DTO.AuthDto;
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

        /*------------------------ Get All Profiles --------------------------*/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var seekers = await seekerService.GetAllAsync();
            return Ok(seekers);
        }

        /*------------------------ Get My Profile --------------------------*/
        [HttpGet("GetMyProfile")]
        public async Task<IActionResult> GetMyProfile()
        {
            if (userId == null)
                return Unauthorized(new ResultDto(false, "User not authenticated"));

            var seekerProfile = await seekerService.GetByUserIdAsync(userId);

            if (seekerProfile == null)
                return NotFound(new ResultDto(false, "Seeker profile not found"));

            return Ok(seekerProfile);
        }

        /*------------------------ Update Profile --------------------------*/
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SeekerProfileUpdateDto dto)
        {
            if (userId == null)
                return Unauthorized(new ResultDto(false, "User not authenticated"));

            var existingProfile = await seekerService.GetByUserIdAsync(userId);
            if (existingProfile == null)
                return NotFound(new ResultDto(false, "Seeker profile not found"));

            var result = await seekerService.UpdateAsync(userId , dto);
            if (!result)
                return BadRequest(result);

            return Ok(result);
        }


		/*------------------------ Upload Files (Profile Image & CV) --------------------------*/
		[HttpPost("upload-files")]
		public async Task<IActionResult> UploadFiles([FromForm] SeekerFileUploadDto dto)
		{
			if (userId == null)
				return Unauthorized(new ResultDto(false, "User not authenticated"));

			var result = await seekerService.UploadFilesAsync(userId, dto);

			if (result.CvUrl == null && result.ProfileImageUrl == null)
				return NotFound(new ResultDto(false, "Seeker profile not found"));

			return Ok(new
			{
				CV_Url = result.CvUrl,
				ProfileImageUrl = result.ProfileImageUrl
			});
		}

		/*------------------------ Delete Profile by Id --------------------------*/
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await seekerService.DeleteAsync(id);
            if (!result)
                return NotFound(result);

            return Ok(result);
        }
    }
}