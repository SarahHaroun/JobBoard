using AutoMapper;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.DTO.UserDto;
using JobBoard.Domain.Entities;
using JobBoard.Repositories.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace JobBoard.Repositories.Data
{
    public static class InitialDataSeeder
    {
        // Method 1: Seed Skills and Categories first
        public static async Task SeedSkillsAndCategories(ApplicationDbContext context)
        {
            Console.WriteLine("=== Starting SeedSkillsAndCategories ===");

            // Seed Skills 
            if (!context.Skills.Any())
            {
                Console.WriteLine("Seeding Skills...");
                var skillsData = File.ReadAllText("../JobBoard.Repositories/Data/DataSeed/skills.json");
                var skills = JsonSerializer.Deserialize<List<Skill>>(skillsData);
                if (skills is not null && skills.Count() > 0)
                {
                    await context.Skills.AddRangeAsync(skills);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Seeded {skills.Count} skills successfully.");
                }
            }
            else
            {
                Console.WriteLine($"Skills already exist: {context.Skills.Count()} skills found.");
            }

            // Seed Categories 
            if (!context.Categories.Any())
            {
                Console.WriteLine("Seeding Categories...");
                var categoriesData = File.ReadAllText("../JobBoard.Repositories/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
                if (categories is not null && categories.Count() > 0)
                {
                    await context.Categories.AddRangeAsync(categories);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Seeded {categories.Count} categories successfully.");
                }
            }
            else
            {
                Console.WriteLine($"Categories already exist: {context.Categories.Count()} categories found.");
            }

            Console.WriteLine("=== Finished SeedSkillsAndCategories ===\n");
        }

        // Method 2: Enhanced Seed Users with Profiles with detailed logging
        public static async Task SeedUsersWithProfiles(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            Console.WriteLine("=== Starting SeedUsersWithProfiles ===");

            try
            {
                // تأكد من وجود الملفات
                var usersPath = "../JobBoard.Repositories/Data/DataSeed/users.json";
                var employersPath = "../JobBoard.Repositories/Data/DataSeed/employers.json";
                var seekersPath = "../JobBoard.Repositories/Data/DataSeed/seekers.json";

                Console.WriteLine($"Checking file paths:");
                Console.WriteLine($"- Users file: {File.Exists(usersPath)} ({usersPath})");
                Console.WriteLine($"- Employers file: {File.Exists(employersPath)} ({employersPath})");
                Console.WriteLine($"- Seekers file: {File.Exists(seekersPath)} ({seekersPath})");

                if (!File.Exists(usersPath) || !File.Exists(employersPath) || !File.Exists(seekersPath))
                {
                    throw new FileNotFoundException("One or more seed files not found");
                }

                // Load JSON files
                var userData = JsonConvert.DeserializeObject<List<UserSeedDto>>(File.ReadAllText(usersPath));
                var employerData = JsonConvert.DeserializeObject<List<EmployerSeedDto>>(File.ReadAllText(employersPath));
                var seekerData = JsonConvert.DeserializeObject<List<SeekerSeedDto>>(File.ReadAllText(seekersPath));

                Console.WriteLine($"Loaded data from JSON:");
                Console.WriteLine($"- Users: {userData?.Count ?? 0}");
                Console.WriteLine($"- Employers: {employerData?.Count ?? 0}");
                Console.WriteLine($"- Seekers: {seekerData?.Count ?? 0}");

                if (userData == null || userData.Count == 0)
                {
                    Console.WriteLine("No user data found or failed to deserialize users.json");
                    return;
                }

                int employersCreated = 0;
                int seekersCreated = 0;
                int usersCreated = 0;

                foreach (var userDto in userData)
                {
                    Console.WriteLine($"Processing user: {userDto.Email} (Type: {userDto.User_Type})");

                    if (await userManager.FindByEmailAsync(userDto.Email) is not null)
                    {
                        Console.WriteLine($"User {userDto.Email} already exists, skipping...");
                        continue;
                    }

                    var user = new ApplicationUser
                    {
                        Email = userDto.Email,
                        UserName = userDto.UserName,
                        PhoneNumber = userDto.PhoneNumber,
                        User_Type = userDto.User_Type
                    };

                    var result = await userManager.CreateAsync(user, userDto.Password);

                    if (!result.Succeeded)
                    {
                        Console.WriteLine($"Failed to create user {userDto.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        continue;
                    }

                    Console.WriteLine($"Successfully created user: {userDto.Email}");
                    usersCreated++;

                    // Link Employer Profile
                    if (userDto.User_Type == UserType.Employer)
                    {
                        var employerDto = employerData?.FirstOrDefault(e => e.UserEmail == userDto.Email);
                        if (employerDto is not null)
                        {
                            Console.WriteLine($"Creating employer profile for: {userDto.Email}");

                            // Use AutoMapper for employer (should work since it's simpler)
                            var employer = mapper.Map<EmployerProfile>(employerDto);
                            employer.UserId = user.Id;

                            context.EmployerProfiles.Add(employer);
                            await context.SaveChangesAsync();
                            employersCreated++;
                            Console.WriteLine($"Employer profile created successfully for: {userDto.Email}");
                        }
                        else
                        {
                            Console.WriteLine($"Warning: No employer data found for user: {userDto.Email}");
                        }
                    }

                    // Link Seeker Profile
                    else if (userDto.User_Type == UserType.Seeker)
                    {
                        var seekerDto = seekerData?.FirstOrDefault(s => s.UserEmail == userDto.Email);
                        if (seekerDto is not null)
                        {
                            Console.WriteLine($"Creating seeker profile for: {userDto.Email}");

                            // Manual mapping instead of AutoMapper for complex properties
                            var seeker = new SeekerProfile
                            {
                                FirstName = seekerDto.FirstName,
                                LastName = seekerDto.LastName,
                                Address = seekerDto.Address,
                                CV_Url = seekerDto.CV_Url,
                                Experience_Level = seekerDto.Experience_Level,
                                Gender = seekerDto.Gender,
                                UserId = user.Id,
                                Skills = new List<Skill>() // Initialize empty, will populate below
                            };

                            // Load skills from db - تأكد من وجود Skills قبل الربط
                            if (seekerDto.Skills != null && seekerDto.Skills.Any())
                            {
                                Console.WriteLine($"Loading skills for seeker: {string.Join(", ", seekerDto.Skills)}");

                                var availableSkills = await context.Skills.ToListAsync();
                                Console.WriteLine($"Available skills in database: {availableSkills.Count}");

                                seeker.Skills = availableSkills
                                    .Where(s => seekerDto.Skills.Contains(s.Id))
                                    .ToList();

                                Console.WriteLine($"Assigned {seeker.Skills.Count} skills to seeker: {userDto.Email}");
                            }
                            else
                            {
                                Console.WriteLine($"No skills assigned to seeker: {userDto.Email}");
                            }

                            context.SeekerProfiles.Add(seeker);
                            await context.SaveChangesAsync();
                            seekersCreated++;
                            Console.WriteLine($"Seeker profile created successfully for: {userDto.Email}");
                        }
                        else
                        {
                            Console.WriteLine($"Warning: No seeker data found for user: {userDto.Email}");
                        }
                    }
                }

                Console.WriteLine($"=== SeedUsersWithProfiles Summary ===");
                Console.WriteLine($"Users created: {usersCreated}");
                Console.WriteLine($"Employers created: {employersCreated}");
                Console.WriteLine($"Seekers created: {seekersCreated}");
                Console.WriteLine($"=== Finished SeedUsersWithProfiles ===\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SeedUsersWithProfiles: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        // Method 3: Enhanced Seed Jobs with detailed logging
        public static async Task SeedJobs(ApplicationDbContext context, IMapper mapper)
        {
            Console.WriteLine("=== Starting SeedJobs ===");

            try
            {
                // Check if jobs already exist
                var existingJobsCount = context.Jobs.Count();
                Console.WriteLine($"Existing jobs in database: {existingJobsCount}");

                if (existingJobsCount > 0)
                {
                    Console.WriteLine("Jobs already exist, skipping job seeding...");
                    return;
                }

                var jobsPath = "../JobBoard.Repositories/Data/DataSeed/jobs.json";

                Console.WriteLine($"Jobs file exists: {File.Exists(jobsPath)} ({jobsPath})");

                if (!File.Exists(jobsPath))
                {
                    Console.WriteLine("Jobs.json file not found");
                    return;
                }

                var jobsData = File.ReadAllText(jobsPath);
                Console.WriteLine($"Jobs file content length: {jobsData.Length} characters");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                var jobSeedDtos = JsonSerializer.Deserialize<List<JobSeedDto>>(jobsData, options);
                Console.WriteLine($"Deserialized jobs count: {jobSeedDtos?.Count ?? 0}");

                if (jobSeedDtos is null || !jobSeedDtos.Any())
                {
                    Console.WriteLine("No jobs to seed or deserialization failed");
                    return;
                }

                // Get existing data from database
                var allSkills = await context.Skills.ToListAsync();
                var allCategories = await context.Categories.ToListAsync();
                var allEmployers = await context.EmployerProfiles.ToListAsync();

                Console.WriteLine($"Available in database:");
                Console.WriteLine($"- Skills: {allSkills.Count}");
                Console.WriteLine($"- Categories: {allCategories.Count}");
                Console.WriteLine($"- Employers: {allEmployers.Count}");

                if (!allEmployers.Any())
                {
                    Console.WriteLine("ERROR: No employers found in database. Cannot create jobs without employers!");
                    return;
                }

                var jobs = new List<Job>();
                int jobsProcessed = 0;

                foreach (var jobSeedDto in jobSeedDtos)
                {
                    jobsProcessed++;
                    Console.WriteLine($"Processing job {jobsProcessed}/{jobSeedDtos.Count}: {jobSeedDto.Title}");

                    // Use AutoMapper for basic property mapping
                    var job = mapper.Map<Job>(jobSeedDto);

                    // Link Job to EmployerProfile using EmployerId from JSON
                    if (jobSeedDto.EmployerId > 0)
                    {
                        var employer = allEmployers.FirstOrDefault(e => e.Id == jobSeedDto.EmployerId);
                        if (employer != null)
                        {
                            job.EmployerId = employer.Id;
                            Console.WriteLine($"Assigned job to employer ID: {employer.Id}");
                        }
                        else
                        {
                            // If employer with specified ID not found, use first available employer
                            var firstEmployer = allEmployers.First();
                            job.EmployerId = firstEmployer.Id;
                            Console.WriteLine($"Employer with ID {jobSeedDto.EmployerId} not found. Using employer ID {firstEmployer.Id} instead.");
                        }
                    }
                    else
                    {
                        // If EmployerId is 0 or negative, assign to first available employer
                        var firstEmployer = allEmployers.First();
                        job.EmployerId = firstEmployer.Id;
                        Console.WriteLine($"No EmployerId specified. Using employer ID {firstEmployer.Id}.");
                    }

                    // Initialize collections
                    job.Skills = new List<Skill>();
                    job.Categories = new List<Category>();

                    // Add skills using SkillIds
                    if (jobSeedDto.SkillIds != null && jobSeedDto.SkillIds.Any())
                    {
                        Console.WriteLine($"Adding skills: {string.Join(", ", jobSeedDto.SkillIds)}");
                        foreach (var skillId in jobSeedDto.SkillIds)
                        {
                            var skill = allSkills.FirstOrDefault(s => s.Id == skillId);
                            if (skill != null)
                            {
                                job.Skills.Add(skill);
                            }
                            else
                            {
                                Console.WriteLine($"Warning: Skill with ID {skillId} not found");
                            }
                        }
                        Console.WriteLine($"Added {job.Skills.Count} skills to job");
                    }

                    // Add categories using CategoryIds
                    if (jobSeedDto.CategoryIds != null && jobSeedDto.CategoryIds.Any())
                    {
                        Console.WriteLine($"Adding categories: {string.Join(", ", jobSeedDto.CategoryIds)}");
                        foreach (var categoryId in jobSeedDto.CategoryIds)
                        {
                            var category = allCategories.FirstOrDefault(c => c.Id == categoryId);
                            if (category != null)
                            {
                                job.Categories.Add(category);
                            }
                            else
                            {
                                Console.WriteLine($"Warning: Category with ID {categoryId} not found");
                            }
                        }
                        Console.WriteLine($"Added {job.Categories.Count} categories to job");
                    }

                    jobs.Add(job);
                    Console.WriteLine($"Job '{jobSeedDto.Title}' prepared for seeding");
                }

                Console.WriteLine($"Adding {jobs.Count} jobs to database...");
                await context.Jobs.AddRangeAsync(jobs);
                await context.SaveChangesAsync();

                Console.WriteLine($"=== SeedJobs Summary ===");
                Console.WriteLine($"Jobs successfully created: {jobs.Count}");
                Console.WriteLine($"=== Finished SeedJobs ===\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SeedJobs: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
