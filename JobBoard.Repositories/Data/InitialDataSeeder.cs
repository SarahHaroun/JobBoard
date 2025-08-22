using AutoMapper;
using JobBoard.Domain.DTO.ApplicationDto;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.DTO.SeekerDto.SeekerSeedDto;
using JobBoard.Domain.DTO.UserDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
using JobBoard.Repositories.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobBoard.Repositories.Data
{
	public static class InitialDataSeeder
	{
		private static readonly string DataPath = "../JobBoard.Repositories/Data/DataSeed";

		// Options for JSON deserialization, case-insensitive and handling enums
		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			Converters = { new JsonStringEnumConverter() }
		};

		// The main entry point for seeding all data
		public static async Task SeedAsync(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IMapper mapper)
		{
			// Seed roles first 
			await SeedRolesAsync(roleManager);

			// Seed skills & categories
			await SeedDataAsync<Skill>(context, context.Skills, "skills.json");
			await SeedDataAsync<Category>(context, context.Categories, "categories.json");

			// Seed all users & their profiles
			await SeedUsersAndProfilesAsync(context, userManager, roleManager, mapper);

			// Seed jobs 
			await SeedJobsAsync(context, mapper);

			// seed applications
			await SeedApplicationsAsync(context, mapper);

		}

		// ================= Jobs =================
		private static async Task SeedJobsAsync(ApplicationDbContext context, IMapper mapper)
		{
			if (await context.Jobs.AnyAsync()) return; 

			var jobDtos = await LoadJsonFileAsync<JobSeedDto>("jobs.json");
			if (jobDtos == null || !jobDtos.Any()) return;

			var allSkills = await context.Skills.ToListAsync(); // get all skills 
			var allCategories = await context.Categories.ToListAsync(); // get categories
			var allEmployers = await context.EmployerProfiles.ToListAsync(); // get employers

			var jobs = jobDtos.Select(dto =>
			{
				var job = mapper.Map<Job>(dto);

				// Match employer by Id or fallback to first one
				job.EmployerId = allEmployers.FirstOrDefault(e => e.Id == dto.EmployerId)?.Id ?? allEmployers.First().Id;

				// Map Skill IDs to Skill entities
				job.Skills = dto.SkillIds?
					.Select(id => allSkills.FirstOrDefault(s => s.Id == id))
					.OfType<Skill>()
					.ToList() ?? new List<Skill>();

				// Map Category IDs to Category entities
				job.Categories = dto.CategoryIds?
					.Select(id => allCategories.FirstOrDefault(c => c.Id == id))
					.OfType<Category>()
					.ToList() ?? new List<Category>();

				return job;
			}).ToList();

			await context.Jobs.AddRangeAsync(jobs);
			await context.SaveChangesAsync();
		}

		// ================= Users & Profiles =================
		private static async Task SeedUsersAndProfilesAsync(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IMapper mapper)
		{
			var users = await LoadJsonFileAsync<UserSeedDto>("users.json");
			var employers = await LoadJsonFileAsync<EmployerSeedDto>("employers.json");
			var seekers = await LoadJsonFileAsync<SeekerSeedDto>("seekers.json");

			if (users == null || employers == null || seekers == null) return;

			foreach (var userDto in users)
			{
				if (await userManager.FindByEmailAsync(userDto.Email) is not null) continue;

				// Create user in Identity
				var user = mapper.Map<ApplicationUser>(userDto);
				user.EmailConfirmed = true;
				var result = await userManager.CreateAsync(user, userDto.Password);
				if (!result.Succeeded) continue;

				// Assign proper role
				var roleName = userDto.User_Type.ToString();
				if (await roleManager.RoleExistsAsync(roleName))
					await userManager.AddToRoleAsync(user, roleName);

				// Employer profile
				if (userDto.User_Type == UserType.Employer)
				{
					var employerDto = employers.FirstOrDefault(e => e.UserEmail == user.Email);
					if (employerDto != null)
					{
						var employer = mapper.Map<EmployerProfile>(employerDto);
						employer.UserId = user.Id;
						context.EmployerProfiles.Add(employer);
						await context.SaveChangesAsync();
					}
				}
				// Seeker profile
				else if (userDto.User_Type == UserType.Seeker)
				{
					var seekerDto = seekers.FirstOrDefault(s => s.UserEmail == user.Email);
					if (seekerDto != null)
					{
						try
						{
							var seeker = mapper.Map<SeekerProfile>(seekerDto);
							seeker.UserId = user.Id;

							// Skills – match IDs from JSON to DB
							if (seekerDto.Skills?.Any() == true)
								seeker.Skills = await context.Skills.Where(s => seekerDto.Skills.Contains(s.Id)).ToListAsync();

							// Certificates, Trainings, Interests, Experiences, Educations
							if (seekerDto.seekerCertificates?.Any() == true)
								seeker.seekerCertificates = mapper.Map<List<SeekerCertificate>>(seekerDto.seekerCertificates);
							if (seekerDto.SeekerTraining?.Any() == true)
								seeker.SeekerTraining = mapper.Map<List<SeekerTraining>>(seekerDto.SeekerTraining);
							if (seekerDto.seekerInterests?.Any() == true)
								seeker.seekerInterests = mapper.Map<List<SeekerInterest>>(seekerDto.seekerInterests);
							if (seekerDto.SeekerExperiences?.Any() == true)
								seeker.SeekerExperiences = mapper.Map<List<SeekerExperience>>(seekerDto.SeekerExperiences);
							if (seekerDto.SeekerEducations?.Any() == true)
								seeker.SeekerEducations = mapper.Map<List<SeekerEducation>>(seekerDto.SeekerEducations);

							context.SeekerProfiles.Add(seeker);
							await context.SaveChangesAsync();
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Error: {ex.Message}");
							throw;
						}
					}
				}
			}
		}

		// ================= Applications =================
		private static async Task SeedApplicationsAsync(ApplicationDbContext context, IMapper mapper)
		{
			if (await context.Applications.AnyAsync()) return;

			var applicationDtos = await LoadJsonFileAsync<ApplicationSeedDto>("applications.json");
			if (applicationDtos == null || !applicationDtos.Any()) return;

			var allApplications = new List<Application>();

			// Get all seekers and jobs for validation
			var allSeekers = await context.SeekerProfiles.ToListAsync();
			var allJobs = await context.Jobs.ToListAsync();

			foreach (var userDto in applicationDtos)
			{
				// Validate if ApplicantId exists
				var seekerExists = allSeekers.Any(s => s.Id == userDto.ApplicantId);
				if (!seekerExists)
				{
					Console.WriteLine($"Skipping applications for ApplicantId {userDto.ApplicantId}: Seeker not found");
					continue;
				}

				foreach (var applicationDto in userDto.Applications)
				{
					// Validate if jobID exists
					var jobExists = allJobs.Any(j => j.Id == applicationDto.JobId);
					if (!jobExists)
					{
						Console.WriteLine($"Skipping application: JobId {applicationDto.JobId} not found");
						continue;
					}

					// Use AutoMapper to map combined data
					var application = mapper.Map<Application>((userDto, applicationDto));
					allApplications.Add(application);
				}
			}

			if (allApplications.Any())
			{
				await context.Applications.AddRangeAsync(allApplications);
				await context.SaveChangesAsync();
			}
		}
		// ================= Helpers =================
		private static async Task<List<T>> LoadJsonFileAsync<T>(string fileName)
		{
			var path = Path.Combine(DataPath, fileName);
			var json = await File.ReadAllTextAsync(path);
			return JsonSerializer.Deserialize<List<T>>(json, JsonOptions);
		}

		private static async Task SeedDataAsync<T>(ApplicationDbContext context, DbSet<T> dbSet, string fileName) where T : class
		{
			if (await dbSet.AnyAsync()) return;

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
					await roleManager.CreateAsync(new IdentityRole(role));
			}
		}
	}
}
