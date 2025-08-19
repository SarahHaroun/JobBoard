
using JobBoard.API.Extensions;
using JobBoard.API.Helpers;

namespace JobBoard.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var configuration = builder.Configuration;

			builder.Services
				.AddDatabaseServices(configuration)
				.AddIdentityServices()
				.AddCustomAuthentication(configuration)
				.AddCustomCors()
				.AddBusinessServices()
				.AddAIServices()
				.AddRedisServices(configuration)
				.AddAutoMapperProfiles()
				.AddApiConfiguration();

			var app = builder.Build();

			await app.InitializeDatabaseAsync();
			app.ConfigureMiddleware();

			app.Run();
		}
	}
}