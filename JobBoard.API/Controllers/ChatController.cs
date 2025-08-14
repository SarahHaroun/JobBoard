using JobBoard.Services.AIChatHistoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatHistoryService _chatHistoryService;

        public ChatController(IChatHistoryService chatHistoryService)
        {
            _chatHistoryService = chatHistoryService;
        }
       

        [HttpPost("{userId}/add")]
        public async Task<IActionResult> AddMessage(string userId, [FromBody] string message)
        {
            await _chatHistoryService.AddMessageAsync(userId, message);
            return Ok("Message added.");
        }

        [HttpGet("{userId}/messages")]
        public async Task<IActionResult> GetMessages(string userId)
        {
            var messages = await _chatHistoryService.GetMessagesAsync(userId);
            return Ok(messages);
        }

        [HttpDelete("{userId}/clear")]
        public async Task<IActionResult> ClearHistory(string userId)
        {
            await _chatHistoryService.ClearHistoryAsync(userId);
            return Ok("Chat history cleared.");
        }
    }
}
