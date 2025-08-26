using JobBoard.API.Extensions;
using JobBoard.API.Helpers;
using JobBoard.Services.CleanupUsersService;

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
                .AddCacheServices(configuration)
                .AddAutoMapperProfiles()
				.AddApiConfiguration();


			builder.Services.AddHostedService<CleanupUnconfirmedUsersService>();
          



            var app = builder.Build();



			await app.InitializeDatabaseAsync();
			app.ConfigureMiddleware();

			app.Run();
		}
	}
}