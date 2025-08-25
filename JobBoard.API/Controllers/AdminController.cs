using JobBoard.Services.AdminService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }


        /////////////////////////get all seekers///////////////////////
        [HttpGet("seekers")]
        public async Task<IActionResult> GetSeekers()
        {
            var seekers = await _adminService.GetAllSeekersAsync();
            if (seekers == null) 
            {
                return NotFound();
            }
            return Ok(seekers);
        }

        /////////////////////////get seeker by id///////////////////////
        [HttpGet("seeker/{seekerId}")]
        public async Task<IActionResult> GetSeekerById(string seekerId)
        {
            var seeker = await _adminService.GetSeekerByIdAsync(seekerId);
            if (seeker == null)
            {
                return NotFound();
            }
            return Ok(seeker);
        }


        /////////////////////////get all employers///////////////////////
        [HttpGet("employers")]
        public async Task<IActionResult> GetEmployers()
        {
            var employers = await _adminService.GetAllEmployersAsync();
            if (employers == null) 
            {
                return NotFound();
            }
            return Ok(employers);
        }

        /////////////////////////get employer by id///////////////////////
        [HttpGet("employer/{employerId}")]
        public async Task<IActionResult> GetEmployerById(string employerId)
        {
            var employer = await _adminService.GetEmployerByIdAsync(employerId);
            if (employer == null)
            {
                return NotFound();
            }
            return Ok(employer);
        }


        /// //////////////////delete user by id///////////////////////
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _adminService.DeleteUserAsync(userId);
            if (!result) return NotFound("User not found.");
            return Ok("User deleted successfully.");
        }


        ////////////////////////get all jobs///////////////////////
        [HttpGet("jobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _adminService.GetAllJobsAsync();
            if (jobs == null)
            {
                return NotFound();
            }
            return Ok(jobs);
        }


        ////////////////////////pending jobs///////////////////////
        [HttpGet("jobs/pending")]
        public async Task<IActionResult> GetPendingJobs()
        {
            var jobs = await _adminService.GetPendingJobsAsync();
            if(jobs == null)
            {
                return NotFound();
            }
            return Ok(jobs);
        }


        // //////////////////approve job by id///////////////////////
        [HttpPut("jobs/{jobId}/approve")]
        public async Task<IActionResult> ApproveJob(int jobId)
        {
            var result = await _adminService.ApproveJobAsync(jobId);
            if (!result) 
                return NotFound("Job not found or already approved.");
            
            return Ok("Job approved successfully.");
        }



        // //////////////////reject job by id///////////////////////
        [HttpDelete("jobs/{jobId}/reject")]
        public async Task<IActionResult> RejectJob(int jobId)
        {
            var result = await _adminService.RejectJobAsync(jobId);
            if (!result) return NotFound("Job not found.");
            return Ok("Job rejected successfully.");
        }



        ////////////////////////get stats///////////////////////
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _adminService.GetStatsAsync();
            return Ok(stats);
        }

		////////////////////////get public"home" stats///////////////////////
		[HttpGet("home-stats")]
        [AllowAnonymous]
		public async Task<IActionResult> GetPublicStats()
		{
			var stats = await _adminService.GetPublicStatsAsync();
			if (stats == null)
			{
				return NotFound();
			}
			return Ok(stats);
		}

		[HttpGet("active-users")]
		[AllowAnonymous]
		public async Task<IActionResult> GetActiveUsers()
		{
			var stats = await _adminService.GetActiveUsersCountAsync();
			return Ok(stats);
		}
	}

}
