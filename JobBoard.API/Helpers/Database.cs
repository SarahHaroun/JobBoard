using JobBoard.Domain.Entities;
using JobBoard.Repositories.Data;
using JobBoard.Repositories.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.API.Helpers
{
	public static class Database
	{
		//----- Configure Entity Framework DbContext -------
		public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			return services;
		}

		//-----Configure Identity System-------

		public static IServiceCollection AddIdentityServices(this IServiceCollection services)
		{
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			return services;
		}


		//----- Apply migrations and seed initial data-------
		
		public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var context = services.GetRequiredService<ApplicationDbContext>();
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				// Apply pending migrations
				var pendingMigrations = context.Database.GetPendingMigrations();
				if (pendingMigrations.Any())
					await context.Database.MigrateAsync();

				// Seed initial data
				var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				var mapper = services.GetRequiredService<AutoMapper.IMapper>();

				await InitialDataSeeder.SeedAsync(context, userManager, roleManager, mapper);
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "An error occurred while migrating the database.");
			}

			return app;
		}
	}
}