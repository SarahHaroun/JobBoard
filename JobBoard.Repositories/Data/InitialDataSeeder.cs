using JobBoard.Domain.Entities;
using JobBoard.Repositories.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Data
{
    public static class InitialDataSeeder
    {

		public static async Task SeedEmployerWithProfile(IServiceProvider serviceProvider)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

			string email = "employer1@example.com";
			string password = "Test@123";
			string role = "Employer";

			// 1. Ensure Role is existed
			if (!await roleManager.RoleExistsAsync(role))
			{
				await roleManager.CreateAsync(new IdentityRole(role));
			}

			// 2.  Ensure User is not existed
			var user = await userManager.FindByEmailAsync(email);
			if (user == null)
			{
				user = new ApplicationUser
				{
					UserName = email,
					Email = email,
					EmailConfirmed = true,
					User_Type = UserType.Employer
				};

				var result = await userManager.CreateAsync(user, password);

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, role);

					// 3. Add EmployerProfile
					var profile = new EmployerProfile
					{
						UserId = user.Id,
						CompanyName = "Tech Corp",
						Website = "https://techcorp.com",
						CompanyLocation = "Cairo, Egypt",
						EstablishedYear = 2010
					};

					context.EmployerProfiles.Add(profile);
					await context.SaveChangesAsync();
				}
				else
				{
					foreach (var error in result.Errors)
						Console.WriteLine($"Error: {error.Description}");
				}
			}
		}

		public static async Task seedAsync(ApplicationDbContext context)
        {
            if (!context.Skills.Any())
            {
                //Read Data from file
                var skillsData = File.ReadAllText("../JobBoard.Repositories/Data/DataSeed/skills.json");

                //2. Convert Json string to list<T>
                var skills = JsonSerializer.Deserialize<List<Skill>>(skillsData);

                //3. Seed Data to Database
                if(skills is not null && skills.Count() > 0)
					await context.Skills.AddRangeAsync(skills);

                await context.SaveChangesAsync();
			}

            if (!context.Categories.Any())
            {
                var categoriesData = File.ReadAllText("../JobBoard.Repositories/Data/DataSeed/categories.json");

				var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                if (categories is not null && categories.Count() > 0)
                    await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }


			if (!context.Jobs.Any())
			{
				var jobsData = File.ReadAllText("../JobBoard.Repositories/Data/DataSeed/jobs.json");

				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
					Converters = { new JsonStringEnumConverter() }
				};

				var jobs = JsonSerializer.Deserialize<List<Job>>(jobsData, options);

				if (jobs is not null && jobs.Count() > 0)
					await context.Jobs.AddRangeAsync(jobs);

				await context.SaveChangesAsync();				}

			///if (!context.EmployerProfiles.Any())
			///{
			///    var employersData = File.ReadAllText("../JobBoard.Repositories/Data/DataSeed/employers.json");
			///    var employers = JsonSerializer.Deserialize<List<EmployerProfile>>(employersData);
			///    if (employers is not null && employers.Count() > 0)
			///        await context.EmployerProfiles.AddRangeAsync(employers);
			///    await context.SaveChangesAsync();
			///}


		}
	}
}
