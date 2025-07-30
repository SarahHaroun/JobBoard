using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Services.AIServices
{
    public interface IGeminiChatService
    {
        Task<string> AskGeminiAsync(string prompt);
        string GetApiKey();
    }
}
