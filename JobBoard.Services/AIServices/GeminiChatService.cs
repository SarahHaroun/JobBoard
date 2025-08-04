using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerativeAI;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace JobBoard.Services.AIServices
{
    public class GeminiChatService : IGeminiChatService
    {
        private readonly string _apiKey;
        private readonly GenerativeModel _model;

        public GeminiChatService(IConfiguration configuration)
        {
            _apiKey = configuration["Gemini:ApiKey"];
            _model = new GenerativeModel(model: "gemini-2.5-flash", apiKey: _apiKey);
        }
        public async Task<string> AskGeminiAsync(string prompt)
        {
            var result = await _model.GenerateContentAsync(prompt);
            return result.Text;
        }

        public string GetApiKey() => _apiKey;
    }
}
