using JobBoard.Services.AIServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AIController : ControllerBase
	{
		private readonly IGeminiChatService _geminiService;

		public AIController(IGeminiChatService geminiService)
		{
			_geminiService = geminiService;
		}

		[HttpGet("ask")]
		public async Task<IActionResult> AskGemini([FromQuery] string prompt)
		{
			var response = await _geminiService.AskGeminiAsync(prompt);
			return Ok(response);
		}
	}
}
