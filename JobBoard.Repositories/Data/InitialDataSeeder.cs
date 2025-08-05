using AutoMapper;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.DTO.JobDto;
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


		public static async Task SeedAsync(ApplicationDbContext context, IMapper mapper)
		{
			// Seed Skills 
			if (!context.Skills.Any())
			{
				var skillsData = File.ReadAllText("../JobBoard.Repositories/Data/DataSeed/skills.json");
				var skills = JsonSerializer.Deserialize<List<Skill>>(skillsData);
				if (skills is not null && skills.Count() > 0)
				{
					await context.Skills.AddRangeAsync(skills);
					await context.SaveChangesAsync();
				}
			}

			// Seed Categories 
			if (!context.Categories.Any())
			{
				var categoriesData = File.ReadAllText("../JobBoard.Repositories/Data/DataSeed/categories.json");
				var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
				if (categories is not null && categories.Count() > 0)
				{
					await context.Categories.AddRangeAsync(categories);
					await context.SaveChangesAsync();
				}
			}

			// Seed Jobs with relationships
			if (!context.Jobs.Any())
			{
				var jobsData = File.ReadAllText("../JobBoard.Repositories/Data/DataSeed/jobs.json");
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
					Converters = { new JsonStringEnumConverter() }
				};

				// Deserialize to JobSeedDto (not JobDto)
				var jobSeedDtos = JsonSerializer.Deserialize<List<JobSeedDto>>(jobsData, options);

				if (jobSeedDtos is not null && jobSeedDtos.Count() > 0)
				{
					// Get existing skills and categories from database
					var allSkills = await context.Skills.ToListAsync();
					var allCategories = await context.Categories.ToListAsync();

					var jobs = new List<Job>();

					foreach (var jobSeedDto in jobSeedDtos)
					{
						// Use AutoMapper for basic property mapping
						var job = mapper.Map<Job>(jobSeedDto);

						// Initialize collections
						job.Skills = new List<Skill>();
						job.Categories = new List<Category>();

						// Add skills using SkillIds
						if (jobSeedDto.SkillIds != null && jobSeedDto.SkillIds.Any())
						{
							foreach (var skillId in jobSeedDto.SkillIds)
							{
								var skill = allSkills.FirstOrDefault(s => s.Id == skillId);
								if (skill != null)
									job.Skills.Add(skill);
							}
						}

						// Add categories using CategoryIds
						if (jobSeedDto.CategoryIds != null && jobSeedDto.CategoryIds.Any())
						{
							foreach (var categoryId in jobSeedDto.CategoryIds)
							{
								var category = allCategories.FirstOrDefault(c => c.Id == categoryId);
								if (category != null)
									job.Categories.Add(category);
							}
						}

						jobs.Add(job);
					}

					await context.Jobs.AddRangeAsync(jobs);
					await context.SaveChangesAsync();
				}
			}
		}
	}
}
