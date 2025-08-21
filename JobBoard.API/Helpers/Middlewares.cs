using JobBoard.API.Hubs;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace JobBoard.API.Extensions
{
	public static class Middlewares
	{
		public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
		{
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			return services;
		}

		public static WebApplication ConfigureMiddleware(this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors("AllowAngularApp");
			app.UseHttpsRedirection();
			app.UseRouting();
            app.UseAuthentication();
			app.UseAuthorization();
			app.UseStaticFiles();
			app.MapControllers();


            /* -----------SignalR MiddlWare------------ */
            app.MapHub<NotificationsHub>("/notifications");

            return app;
		}
	}
}