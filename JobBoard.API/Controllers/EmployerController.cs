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
        private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        private readonly IEmployerService employerService;

        public EmployerController(IEmployerService employerService)
        {
            this.employerService = employerService;
        }


        /*------------------------Get All Profiles --------------------------*/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await employerService.GetAll();
            return Ok(result);
        }


        /*------------------------Get my Profile --------------------------*/
        [HttpGet("GetMyProfile")]
        public async Task<IActionResult> GetById()
        {
            if (userId == null)
                return Unauthorized();

            var result =await employerService.GetByUserIdAsync(userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        /*------------------------create --------------------------*/
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmpProfileDto dto)
        {
            if (userId == null)
                return Unauthorized();

            dto.UserId = userId;

            var result = await employerService.Create(dto);
            //return create("URLNAME of Employer" , EmployerCreated)
            return Ok(result);
        }


        /*------------------------Update --------------------------*/
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmpProfileDto dto)
        {
            if (userId == null)
                return Unauthorized();

            var myProfile = await employerService.GetByUserIdAsync(userId);
            if (myProfile == null)
                return NotFound();

            var result = await employerService.Update(myProfile.Id, dto);
            if (result)
                return Ok("updated");

            return NotFound();
        }

        /*------------------------Delete --------------------------*/
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (userId == null) return Unauthorized();

            var myProfile = employerService.GetByUserIdAsync(userId);
            if (myProfile == null) return NotFound();

            var deleted = await employerService.DeleteById(myProfile.Id);
            return deleted ? Ok("Deleted") : BadRequest();
        }

    }
}
