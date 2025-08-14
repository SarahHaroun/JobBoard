using AutoMapper;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.DTO.UserDto;
using JobBoard.Domain.Entities;
using JobBoard.Repositories.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobBoard.Repositories.Data
{
	public static class InitialDataSeeder
	{
		//Path to folder containing JSON seed files
		private static readonly string DataPath = "../JobBoard.Repositories/Data/DataSeed";

		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			Converters = { new JsonStringEnumConverter() }
		};

		//Main method for seeding data
		public static async Task SeedAsync(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IMapper mapper)
		{
			await SeedRolesAsync(roleManager);
			await SeedDataAsync<Skill>(context, context.Skills, "skills.json");
			await SeedDataAsync<Category>(context, context.Categories, "categories.json");
			await SeedUsersAndProfilesAsync(context, userManager, roleManager, mapper);
			await SeedJobsAsync(context, mapper);
		}

		//Seeds Jobs
		private static async Task SeedJobsAsync(ApplicationDbContext context, IMapper mapper)
		{
			if (await context.Jobs.AnyAsync())
				return;

			//Load job data from JSON
			var jobDtos = await LoadJsonFileAsync<JobSeedDto>("jobs.json");
			if (jobDtos == null || !jobDtos.Any()) return;

			//Load related data from DB to match relationships
			var allSkills = await context.Skills.ToListAsync();
			var allCategories = await context.Categories.ToListAsync();
			var allEmployers = await context.EmployerProfiles.ToListAsync();

			//Map DTOs to entities
			var jobs = jobDtos.Select(dto =>
			{
				var job = mapper.Map<Job>(dto);

				job.EmployerId = allEmployers.FirstOrDefault(e => e.Id == dto.EmployerId)?.Id ?? allEmployers.First().Id;

				//Map Skill IDs to Skill entities
				job.Skills = dto.SkillIds?
					.Select(id => allSkills.FirstOrDefault(s => s.Id == id))
					.OfType<Skill>()
					.ToList() ?? new List<Skill>();

				//Map Category IDs to Category entities
				job.Categories = dto.CategoryIds?
					.Select(id => allCategories.FirstOrDefault(c => c.Id == id))
					.OfType<Category>()
					.ToList() ?? new List<Category>();

				return job;
			}).ToList();

			//Add jobs to DB
			await context.Jobs.AddRangeAsync(jobs);
			await context.SaveChangesAsync();
		}

		//Seeds Users and their related profiles (Employer or Seeker)
		private static async Task SeedUsersAndProfilesAsync(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IMapper mapper)
		{
			// Load users and profile data
			var users = await LoadJsonFileAsync<UserSeedDto>("users.json");
			var employers = await LoadJsonFileAsync<EmployerSeedDto>("employers.json");
			var seekers = await LoadJsonFileAsync<SeekerSeedDto>("seekers.json");

			if (users == null || employers == null || seekers == null) return;

			foreach (var userDto in users)
			{
				if (await userManager.FindByEmailAsync(userDto.Email) is not null)
					continue;

				// Create ApplicationUser
				var user = mapper.Map<ApplicationUser>(userDto);
				user.EmailConfirmed = true;
				var result = await userManager.CreateAsync(user, userDto.Password);
				if (!result.Succeeded) continue;

				// Assign role to user (e.g., "Employer", "Seeker")
				var roleName = userDto.User_Type.ToString();
				if (await roleManager.RoleExistsAsync(roleName))
				{
					await userManager.AddToRoleAsync(user, roleName);
				}

				// Create EmployerProfile if user is Employer
				if (userDto.User_Type == UserType.Employer)
				{
					var employerDto = employers.FirstOrDefault(e => e.UserEmail == user.Email);
					if (employerDto != null)
					{
						var employer = mapper.Map<EmployerProfile>(employerDto);
						employer.UserId = user.Id;
						context.EmployerProfiles.Add(employer);
					}
				}
				// Create SeekerProfile if user is Seeker
				else if (userDto.User_Type == UserType.Seeker)
				{
					var seekerDto = seekers.FirstOrDefault(s => s.UserEmail == user.Email);
					if (seekerDto != null)
					{
						var seeker = mapper.Map<SeekerProfile>(seekerDto);
						seeker.UserId = user.Id;

						// Assign skills to seeker
						seeker.Skills = await context.Skills
							.Where(skill => seekerDto.Skills.Contains(skill.Id))
							.ToListAsync();

						context.SeekerProfiles.Add(seeker);
					}
				}
			}

			await context.SaveChangesAsync();
		}

		//Load list<T> from JSON file
		private static async Task<List<T>> LoadJsonFileAsync<T>(string fileName)
		{
			var path = Path.Combine(DataPath, fileName);
			var json = await File.ReadAllTextAsync(path);
			return JsonSerializer.Deserialize<List<T>>(json, JsonOptions);
		}

		//Seed any table if it's empty
		private static async Task SeedDataAsync<T>(ApplicationDbContext context, DbSet<T> dbSet, string fileName) where T : class
		{
			if (await dbSet.AnyAsync()) return;

			// Load and insert data
			var items = await LoadJsonFileAsync<T>(fileName);
			if (items == null || !items.Any()) return;

			await dbSet.AddRangeAsync(items);
			await context.SaveChangesAsync();
		}

		private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			var roles = new[] { "Admin", "Employer", "Seeker" };

			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}
		}

	}
}