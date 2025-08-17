using JobBoard.Domain.Services.Contract;
using JobBoard.Services.AIChatHistoryServices;
using JobBoard.Services.AIEmbeddingService;
using JobBoard.Services.AIServices;

namespace JobBoard.API.Extensions
{
	public static class AIServices
	{
		public static IServiceCollection AddAIServices(this IServiceCollection services)
		{
			services.AddSingleton<IGeminiChatService, GeminiChatService>();
			services.AddScoped<IAIEmbeddingService, AIEmbeddingService>();
			services.AddScoped<IChatHistoryService, ChatHistoryService>();

			return services;
		}
	}
}