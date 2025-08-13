using AutoMapper;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Mapping;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Domain.Services.Contract;
using JobBoard.Repositories;
using JobBoard.Repositories.Data;
using JobBoard.Repositories.Persistence;
using JobBoard.Services;
using JobBoard.Services._ِAuthService;
using JobBoard.Services.AIEmbeddingService;
using JobBoard.Services.AIServices;
using JobBoard.Services.CategoryService;
using JobBoard.Services.EmailService;
using JobBoard.Services.EmployerService;
using JobBoard.Services.SeekerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using JobBoard.Domain.Mapping.Resolvers;

namespace JobBoard.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var configuration = builder.Configuration;

			/*------------------------Add DbContext--------------------------*/
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            /*------------------------Add Identity--------------------------*/
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();


			/*------------------------Add JWT Authentication--------------------------*/
			 builder.Services.AddAuthentication(options =>
			 {
				 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  //return unAuthorized
				 options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			 })
			 .AddJwtBearer(options =>         /// Validate the token
			 {
				 options.RequireHttpsMetadata = true; // Use HTTPS for production
				 options.SaveToken = true; // Save the token in the request
				 options.TokenValidationParameters = new TokenValidationParameters
				 {
					 ValidateIssuer = true,
					 ValidateAudience = true,
					 ValidateLifetime = true,
					 ValidateIssuerSigningKey = true,
					 ValidIssuer = configuration["JWT:ValidIssuer"],
					 ValidAudience = configuration["JWT:ValidAudience"],
					 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])) // Secret key for signing the token
				 };
			 })
              .AddGoogle(options =>
                {
                    options.ClientId = configuration["GoogleAuthSettings:ClientId"]!;
                    options.ClientSecret = configuration["GoogleAuthSettings:ClientSecret"]!;
                }); ;

			/*------------------------Add Services--------------------------*/

			/*-------------------- Cors Policy --------------------*/
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAngularApp", policy =>
				{
					policy.WithOrigins("http://localhost:4200")
						  .AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowCredentials();
				});
			});

			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IEmployerService, EmployerService>();
			builder.Services.AddScoped<ISeekerService, SeekerService>();
			builder.Services.AddScoped<IJobService, JobService>();
			builder.Services.AddScoped<ISeekerService, SeekerService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmployerService, EmployerService>();
            builder.Services.AddScoped<ISeekerService, SeekerService>();
            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<ISeekerService, SeekerService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();


			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			/*-------------------- Add Ai Service ---------------------*/

			builder.Services.AddSingleton<IGeminiChatService, GeminiChatService>();
			builder.Services.AddScoped<IAIEmbeddingService, AIEmbeddingService>();

			/*--------------- Add Services AutoMappper Profiles ---------------*/
			builder.Services.AddAutoMapper(cfg =>
			{
				cfg.AddProfile<JobProfile>();
				cfg.AddProfile<EmployerProfileMapping>();
				cfg.AddProfile<UserProfileMapping>();
				cfg.AddProfile<SkillAndCategoryProfile>();
				cfg.AddProfile<ApplicationProfile>();
				cfg.AddProfile<SeekerProfileMapping>();
				cfg.AddProfile<SavedJobProfile>();
			});

			builder.Services.AddScoped<CompanyImageUrlResolver>();
			builder.Services.AddScoped<SeekerCvUrlResolver>();

			builder.Services.AddAuthorization();
			builder.Services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});
			//builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			//Update & Seed Database 
			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var context = services.GetRequiredService<ApplicationDbContext>();
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			try
			{
				var pendingMigrations = context.Database.GetPendingMigrations();
				if (pendingMigrations.Any())
					await context.Database.MigrateAsync();


				var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				var mapper = services.GetRequiredService<IMapper>();

				await InitialDataSeeder.SeedAsync(context, userManager, roleManager, mapper);
			}
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "An error occurred while migrating the database.");
			}

			// Configure Middleware Pipeline
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

			app.Run();
		}
	}
}