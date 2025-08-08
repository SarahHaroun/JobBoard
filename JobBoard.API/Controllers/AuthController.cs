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

            var result = await authService.ExternalLoginAsync(dto.IdToken, dto.RoleFromClient);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("test-google-connection")]
        public async Task<IActionResult> TestGoogleConnection()
        {
            using var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.GetAsync("https://oauth2.googleapis.com/tokeninfo");
                if (response.IsSuccessStatusCode)
                {
                    return Ok("✅ الاتصال بـ Google شغال تمام.");
                }
                else
                {
                    return BadRequest($"❌ الاتصال بـ Google رجّع status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ حصل exception: {ex.Message}");
            }
        }


    }


}
