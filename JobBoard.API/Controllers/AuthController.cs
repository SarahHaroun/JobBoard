using JobBoard.Domain.Entities;
using JobBoard.Domain.DTO.AuthDto;
using JobBoard.Services._ِAuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobBoard.Domain.DTO.ExternalLoginDto;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService authService;

		public AuthController(IAuthService authService)
		{
			this.authService = authService;
		}

		/*------------------------Register--------------------------*/

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authService.RegisterAsync(model);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }

		}

        /*------------------------Login--------------------------*/
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authService.LoginAsync(model);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized(result.Message);
            }
        }


        /*--------------------------------External login------------------------------------------*/
        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginReceiverDto dto)
        {
            Console.WriteLine($"Server Time: {DateTime.UtcNow}");
            var result = await authService.ExternalLoginAsync(dto);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        /*------------------------Change Password--------------------------*/
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authService.ChangePasswordAsync(model);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }


        /*------------------------Reset Password--------------------------*/
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authService.ResetPasswordAsync(model);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /*------------------------Forget Password--------------------------*/
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPassDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authService.ForgotPasswordAsync(model);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        ///*------------------------Send Confirm Email--------------------------*/
        [HttpPost("send-confirmation-email")]
        public async Task<IActionResult> SendConfirmationEmail([FromBody] string email)
        {
            var result = await authService.SendConfirmationEmailAsync(email);
            return result.Succeeded ? Ok(result) : BadRequest(result.Message);
        }


        /*------------------------ Confirm Email--------------------------*/
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto model)
        {
            var result = await authService.ConfirmEmailAsync(model);
            return result.Succeeded ? Ok(result) : BadRequest(result.Message);
        }



    }
}


 


