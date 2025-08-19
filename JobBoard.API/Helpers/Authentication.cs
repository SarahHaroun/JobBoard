using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JobBoard.API.Extensions
{
	public static class AuthenticationExtensions
	{
		public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = true;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["JWT:ValidIssuer"],
					ValidAudience = configuration["JWT:ValidAudience"],
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
				};
			})
			.AddGoogle(options =>
			{
				options.ClientId = configuration["GoogleAuthSettings:ClientId"]!;
				options.ClientSecret = configuration["GoogleAuthSettings:ClientSecret"]!;
			});

			services.AddAuthorization();

			return services;
		}

		public static IServiceCollection AddCustomCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAngularApp", policy =>
				{
					policy.WithOrigins("http://localhost:4200")
						  .AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowCredentials();
				});
			});

			return services;
		}
	}
}