using JobBoard.Domain.Services.Contract;
using JobBoard.Repositories.Redis;
using StackExchange.Redis;

namespace JobBoard.API.Extensions
{
	public static class RedisExtensions
	{
		public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IConnectionMultiplexer>(sp =>
			{
				var connectionString = configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connectionString!);
			});

			services.AddSingleton<IRedisService, RedisService>();

			return services;
		}
	}
}