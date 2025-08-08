using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Services.EmployerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Employer")]
    public class EmployerController : ControllerBase
    {
        private string? userId => User.FindFirstValue(ClaimTypes.NameIdentifier);

		private readonly IEmployerService employerService;

		public EmployerController(IEmployerService employerService)
		{
			this.employerService = employerService;
		}


        /*------------------------Get All Profiles --------------------------*/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {      
            if (userId == null)
                return Unauthorized();
            var result = await employerService.GetAll();
            if (result == null || !result.Any())
                return NotFound("No employer profiles found");
            return Ok(result);
        }


        /*------------------------Get my Profile --------------------------*/
        [HttpGet("my-profile")]
        public async Task<IActionResult> GetById()
        {
            if (userId == null)
                return Unauthorized();

			var result = await employerService.GetByUserId(userId);

			if (result == null)
				return NotFound();

			return Ok(result);
		}


		/*------------------------create --------------------------*/
		//[HttpPost]
		//public async Task<IActionResult> Create([FromBody] EmpProfileDto dto)
		//{
		//    if (userId == null)
		//        return Unauthorized();

		//    var existingProfile = await employerService.GetByUserId(userId);
		//    if (existingProfile != null)
		//        return BadRequest("Employer profile already exists");



		/*------------------------Update --------------------------*/
		[HttpPut]
		public async Task<IActionResult> Update([FromBody] EmpProfileUpdateDto dto)
		{
			if (userId == null)
				return Unauthorized();

			var updatedProfile = await employerService.GetByUserId(userId);
			if (updatedProfile == null)
				return NotFound();

			bool updated = await employerService.Update(updatedProfile.Id, dto);

			return updated ? Ok("updated") : NotFound();
		}

		/*------------------------Delete --------------------------*/
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			if (userId == null)
				return Unauthorized();

			var deletedProfile = await employerService.GetByUserId(userId);
			if (deletedProfile == null)
				return NotFound();

			bool deleted = await employerService.DeleteById(deletedProfile.Id);
			return deleted ? Ok("Deleted") : BadRequest();
		}

	}
}
