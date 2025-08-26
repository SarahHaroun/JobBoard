using JobBoard.API.Hubs;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
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
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Ensure camelCase
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


            app.UseRouting();
            app.UseCors("AllowAngularApp");
			app.UseHttpsRedirection();
			
            app.UseAuthentication();
			app.UseAuthorization();
			app.UseStaticFiles();
            app.UseOutputCache(); // Enable Output Caching Middleware
            app.MapControllers();


            /* -----------SignalR MiddlWare------------ */
            app.MapHub<NotificationsHub>("/notifications");

            return app;
		}
	}
}